using System.Media;
using NAudio.Wave;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace music
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            BackColor = Color.Black;
            ClientSize = new Size(1024, 768);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Init();

        }

        Image mHead;//中间头像
        IWavePlayer mPlayer;//播放器
        AudioFileReader mWavReader;//音频采样使用
        AudioFileReader mFileReader;//音频播放使用
        int mSampleRate = 0; // 采样率
        int mChannelCount = 1;//声道数量
        int mAccTickCount = 0;//累计采样次数
        int mTotalSamples = 0;//总样本数
        int mAccSamples = 0;//累计样本数
        double mExpectTickMs = 0;//理论上每帧采样时间
        double mStartTickMs = 0;//开始采样时间
        double mLastTickMs = 0;//上次tick时间
        float[] mFloatBuffer;//双通道采样缓存
        float[] mSamples;//平均后的采样点
        Complex[] mFFTBuffer;//频域数据
        const int mFFTLength = 1024; // FFT长度
        const int mLineCount = 64; //柱的数量
        float[] mLineHeights = new float[mLineCount];//柱子高度
        float[] mMaxHeights = new float[mLineCount];//柱子最大高度
        float mCircleRadius = 100;//内圆半径
        float mLineWidth = 10;//宽度
        float mFallingSpeed = 0.95f;//高点回落速度
        PointF[] mCurvePoints;//波浪曲线
        Stopwatch mStopwatch = new Stopwatch();//采样计时器

        void Init()
        {
            Environment.CurrentDirectory = Environment.CurrentDirectory + "/../../../";
            mHead = new Bitmap("head.png");
            mFileReader = new AudioFileReader("music.wav");
            mPlayer = new WaveOutEvent();
            mPlayer.Init(mFileReader);
            mFFTBuffer = new Complex[mFFTLength];//傅里叶变换用的缓冲区
            mSamples = new float[mFFTLength];//最终样本缓冲区
            mCurvePoints = new PointF[mFFTLength];//波浪线采样点
            mWavReader = new AudioFileReader("music.wav");
            mChannelCount = mWavReader.WaveFormat.Channels;//通道数
            mSampleRate = mWavReader.WaveFormat.SampleRate;//采样率
            int bytesPerSample = mWavReader.WaveFormat.BitsPerSample / 8;//每个样本字节数
            mTotalSamples = (int)(1.0 * mWavReader.Length / bytesPerSample / mChannelCount);//总样本数
            mExpectTickMs = mFFTLength * 1000.0 / mSampleRate;//采样1024个样本经过的时间
            mFloatBuffer = new float[mChannelCount * mFFTLength];//采样缓冲区
            mLineWidth = MathF.Min(10, MathF.Max(1, 1.5f * MathF.PI * mCircleRadius / mLineHeights.Length));

            timer1.Interval = 1;//尽可能小的采样周期
            timer1.Tick += Tick;
        }

        bool mIsPlaying = false;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(mIsPlaying) { return; }
            if(e.KeyCode == Keys.Space)
            {
                mIsPlaying = true;
                timer1.Start();
                mPlayer.Play();//开始播放音乐
                mStopwatch.Start();
            }
        }

        private void Tick(object? sender, EventArgs e)
        {
            //如果音乐播放没开始就不开始采样
            if (mFileReader.Position == 0)
            {
                return;
            }
            //如果当前时间小于已经采样到的时间就停止采样
            if (mStopwatch.Elapsed.TotalMilliseconds - mStartTickMs < mAccTickCount * mExpectTickMs)
            {
                return;
            }
            //累计采样次数
            mAccTickCount++;

            //累计样本数小于总样本数
            if (mAccSamples < mTotalSamples)
            {
                //每次采样（通道数*1024）个样本
                int samplesRead = mWavReader.Read(mFloatBuffer, 0, mFloatBuffer.Length);
                mAccSamples += samplesRead / mChannelCount;
                //计算样本平均值
                int j = 0;
                for (int i = 0; i < samplesRead; i += mChannelCount)
                {
                    float avg = mFloatBuffer[i];
                    if (mChannelCount == 2)
                    {
                        avg = (mFloatBuffer[i] + mFloatBuffer[i + 1]) / 2;
                    }
                    mSamples[j] = avg;
                    mFFTBuffer[j++] = new Complex(avg, 0);
                }
                //如果样本不足1024个就补充0
                for (; j < mFFTLength; j++)
                {
                    mFFTBuffer[j] = new Complex(0, 0);
                }
                //傅里叶变换
                Fourier.Forward(mFFTBuffer);
                //复数的模表示频率的幅值，把它作为柱状图的高
                for (int i = 0; i < mLineHeights.Length; i++)
                {
                    mLineHeights[i] = MathF.Sqrt((float)mFFTBuffer[i].Magnitude);//向1逼近
                }
            }
            Invalidate();
            mLastTickMs = mStopwatch.Elapsed.TotalMilliseconds;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = 0; i < mLineHeights.Length; i++)
            {
                //绘制柱子
                float angle = i * 360 / mLineHeights.Length;
                float t = angle * MathF.PI / 180;
                PointF p0 = new PointF(mCircleRadius * MathF.Cos(t), mCircleRadius * MathF.Sin(t)) + ClientSize / 2;
                PointF p1 = new PointF(mCircleRadius * (1 + mLineHeights[i]) * MathF.Cos(t), mCircleRadius * (1 + mLineHeights[i]) * MathF.Sin(t)) + ClientSize / 2;
                //根据柱子的角度在颜色频谱上采样（HSV颜色）再转换成RGB颜色显示
                Pen pen = new Pen(ColorHelper.HSVtoRGB(angle, 1, 0.9f), mLineWidth);
                e.Graphics.DrawLine(pen, p0, p1);

                //绘制最高点
                if (mLineHeights[i] > mMaxHeights[i])
                {
                    mMaxHeights[i] = mLineHeights[i];
                }

                float topWdith = MathF.Min(3, MathF.Max(mLineWidth / 2, 1));
                PointF pTop1 = new PointF(mCircleRadius * (1 + mMaxHeights[i]) * MathF.Cos(t), mCircleRadius * (1 + mMaxHeights[i]) * MathF.Sin(t)) + ClientSize / 2;
                PointF pTop2 = new PointF((mCircleRadius * (1 + mMaxHeights[i]) + topWdith) * MathF.Cos(t), (mCircleRadius * (1 + mMaxHeights[i]) + topWdith) * MathF.Sin(t)) + ClientSize / 2;
                Pen topPen = new Pen(Color.White, mLineWidth);
                e.Graphics.DrawLine(topPen, pTop1, pTop2);
                mMaxHeights[i] *= mFallingSpeed;
                if (mMaxHeights[i] < 0.001) mMaxHeights[i] = 0;
            }

            //统计
            //e.Graphics.DrawString($"use tick:{accTicks} samples:{accSamples}/{totalSamples} use time:{sw.Ellapsed}", SystemFonts.DefaultFont, Brushes.AliceBlue, new PointF(100, 100));
            double fps = 1000.0 / (mStopwatch.Elapsed.TotalMilliseconds - mLastTickMs);
            e.Graphics.DrawString($"{fps:0.00} FPS", SystemFonts.DefaultFont, Brushes.YellowGreen, new PointF(10, 10));
            //头像
            Matrix matrix = e.Graphics.Transform;
            e.Graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            e.Graphics.RotateTransform(mStopwatch.ElapsedMilliseconds / 100 % 360);
            e.Graphics.DrawImage(mHead, new Rectangle(-mHead.Width / 2, -mHead.Height / 2, mHead.Width, mHead.Height));
            e.Graphics.Transform = matrix;

            //波浪
            for (int i = 0; i < mFFTLength; i++)
            {
                float v = mSamples[i];
                mCurvePoints[i] = new PointF(i, v*30+ClientSize.Height/7*6);
            }
            e.Graphics.DrawCurve(new Pen(Color.SkyBlue,2), mCurvePoints);
        }

    }

    /// <summary>
    /// 颜色转换帮助类
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        ///  RGB转HSV
        /// </summary>
        /// <param name="red">红色值</param>
        /// <param name="green">绿色值</param>
        /// <param name="blue">蓝色值</param>
        /// <returns>返回：HSV值集合</returns>
        public static List<float> RGBtoHSV(int red, int green, int blue)
        {
            List<float> hsbList = new List<float>();
            System.Drawing.Color dColor = System.Drawing.Color.FromArgb(red, green, blue);
            hsbList.Add(dColor.GetHue());
            hsbList.Add(dColor.GetSaturation());
            hsbList.Add(dColor.GetBrightness());
            return hsbList;
        }


        /// <summary>
        /// HSV转RGB
        /// </summary>
        /// <param name="hue">色调</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="brightness">亮度</param>
        /// <returns>返回：Color</returns>
        public static Color HSVtoRGB(float hue, float saturation, float brightness)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = hue / 60f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }
}
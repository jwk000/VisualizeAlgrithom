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

        Image mHead;//�м�ͷ��
        IWavePlayer mPlayer;//������
        AudioFileReader mWavReader;//��Ƶ����ʹ��
        AudioFileReader mFileReader;//��Ƶ����ʹ��
        int mSampleRate = 0; // ������
        int mChannelCount = 1;//��������
        int mAccTickCount = 0;//�ۼƲ�������
        int mTotalSamples = 0;//��������
        int mAccSamples = 0;//�ۼ�������
        double mExpectTickMs = 0;//������ÿ֡����ʱ��
        double mStartTickMs = 0;//��ʼ����ʱ��
        double mLastTickMs = 0;//�ϴ�tickʱ��
        float[] mFloatBuffer;//˫ͨ����������
        float[] mSamples;//ƽ����Ĳ�����
        Complex[] mFFTBuffer;//Ƶ������
        const int mFFTLength = 1024; // FFT����
        const int mLineCount = 64; //��������
        float[] mLineHeights = new float[mLineCount];//���Ӹ߶�
        float[] mMaxHeights = new float[mLineCount];//�������߶�
        float mCircleRadius = 100;//��Բ�뾶
        float mLineWidth = 10;//���
        float mFallingSpeed = 0.95f;//�ߵ�����ٶ�
        PointF[] mCurvePoints;//��������
        Stopwatch mStopwatch = new Stopwatch();//������ʱ��

        void Init()
        {
            Environment.CurrentDirectory = Environment.CurrentDirectory + "/../../../";
            mHead = new Bitmap("head.png");
            mFileReader = new AudioFileReader("music.wav");
            mPlayer = new WaveOutEvent();
            mPlayer.Init(mFileReader);
            mFFTBuffer = new Complex[mFFTLength];//����Ҷ�任�õĻ�����
            mSamples = new float[mFFTLength];//��������������
            mCurvePoints = new PointF[mFFTLength];//�����߲�����
            mWavReader = new AudioFileReader("music.wav");
            mChannelCount = mWavReader.WaveFormat.Channels;//ͨ����
            mSampleRate = mWavReader.WaveFormat.SampleRate;//������
            int bytesPerSample = mWavReader.WaveFormat.BitsPerSample / 8;//ÿ�������ֽ���
            mTotalSamples = (int)(1.0 * mWavReader.Length / bytesPerSample / mChannelCount);//��������
            mExpectTickMs = mFFTLength * 1000.0 / mSampleRate;//����1024������������ʱ��
            mFloatBuffer = new float[mChannelCount * mFFTLength];//����������
            mLineWidth = MathF.Min(10, MathF.Max(1, 1.5f * MathF.PI * mCircleRadius / mLineHeights.Length));

            timer1.Interval = 1;//������С�Ĳ�������
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
                mPlayer.Play();//��ʼ��������
                mStopwatch.Start();
            }
        }

        private void Tick(object? sender, EventArgs e)
        {
            //������ֲ���û��ʼ�Ͳ���ʼ����
            if (mFileReader.Position == 0)
            {
                return;
            }
            //�����ǰʱ��С���Ѿ���������ʱ���ֹͣ����
            if (mStopwatch.Elapsed.TotalMilliseconds - mStartTickMs < mAccTickCount * mExpectTickMs)
            {
                return;
            }
            //�ۼƲ�������
            mAccTickCount++;

            //�ۼ�������С����������
            if (mAccSamples < mTotalSamples)
            {
                //ÿ�β�����ͨ����*1024��������
                int samplesRead = mWavReader.Read(mFloatBuffer, 0, mFloatBuffer.Length);
                mAccSamples += samplesRead / mChannelCount;
                //��������ƽ��ֵ
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
                //�����������1024���Ͳ���0
                for (; j < mFFTLength; j++)
                {
                    mFFTBuffer[j] = new Complex(0, 0);
                }
                //����Ҷ�任
                Fourier.Forward(mFFTBuffer);
                //������ģ��ʾƵ�ʵķ�ֵ��������Ϊ��״ͼ�ĸ�
                for (int i = 0; i < mLineHeights.Length; i++)
                {
                    mLineHeights[i] = MathF.Sqrt((float)mFFTBuffer[i].Magnitude);//��1�ƽ�
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
                //��������
                float angle = i * 360 / mLineHeights.Length;
                float t = angle * MathF.PI / 180;
                PointF p0 = new PointF(mCircleRadius * MathF.Cos(t), mCircleRadius * MathF.Sin(t)) + ClientSize / 2;
                PointF p1 = new PointF(mCircleRadius * (1 + mLineHeights[i]) * MathF.Cos(t), mCircleRadius * (1 + mLineHeights[i]) * MathF.Sin(t)) + ClientSize / 2;
                //�������ӵĽǶ�����ɫƵ���ϲ�����HSV��ɫ����ת����RGB��ɫ��ʾ
                Pen pen = new Pen(ColorHelper.HSVtoRGB(angle, 1, 0.9f), mLineWidth);
                e.Graphics.DrawLine(pen, p0, p1);

                //������ߵ�
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

            //ͳ��
            //e.Graphics.DrawString($"use tick:{accTicks} samples:{accSamples}/{totalSamples} use time:{sw.Ellapsed}", SystemFonts.DefaultFont, Brushes.AliceBlue, new PointF(100, 100));
            double fps = 1000.0 / (mStopwatch.Elapsed.TotalMilliseconds - mLastTickMs);
            e.Graphics.DrawString($"{fps:0.00} FPS", SystemFonts.DefaultFont, Brushes.YellowGreen, new PointF(10, 10));
            //ͷ��
            Matrix matrix = e.Graphics.Transform;
            e.Graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            e.Graphics.RotateTransform(mStopwatch.ElapsedMilliseconds / 100 % 360);
            e.Graphics.DrawImage(mHead, new Rectangle(-mHead.Width / 2, -mHead.Height / 2, mHead.Width, mHead.Height));
            e.Graphics.Transform = matrix;

            //����
            for (int i = 0; i < mFFTLength; i++)
            {
                float v = mSamples[i];
                mCurvePoints[i] = new PointF(i, v*30+ClientSize.Height/7*6);
            }
            e.Graphics.DrawCurve(new Pen(Color.SkyBlue,2), mCurvePoints);
        }

    }

    /// <summary>
    /// ��ɫת��������
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        ///  RGBתHSV
        /// </summary>
        /// <param name="red">��ɫֵ</param>
        /// <param name="green">��ɫֵ</param>
        /// <param name="blue">��ɫֵ</param>
        /// <returns>���أ�HSVֵ����</returns>
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
        /// HSVתRGB
        /// </summary>
        /// <param name="hue">ɫ��</param>
        /// <param name="saturation">���Ͷ�</param>
        /// <param name="brightness">����</param>
        /// <returns>���أ�Color</returns>
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
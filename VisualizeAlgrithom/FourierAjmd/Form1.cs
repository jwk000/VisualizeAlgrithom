using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace fourier
{
    //用傅里叶变换绘制阿基米德曲线，按空格键开始
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            BackColor = Color.Black;
            ClientSize = new Size(1024, 768);

        }

        const int FFTLength = 1024;
        const int CircleNum = 1024;
        Complex[] FFTBuffer = new Complex[FFTLength];
        PointF[] SamplePoints = new PointF[FFTLength];
        List<PointF> Path = new List<PointF>();
        PointF[] center = new PointF[CircleNum+1];
        float[] radius = new float[CircleNum];
        List<List<Complex>> CirclePaths;
        void Sample()
        {
            //生成采样点
            int a = 100;
            int b = 100;
            for (int i = 0; i < FFTLength; i++)
            {
                float t = i* MathF.PI / 360;
                float x = (a + b * t) * MathF.Cos(t);
                float y = (a + b * t) * MathF.Sin(t);
                FFTBuffer[i] = new Complex(x,y);
                SamplePoints[i]=new PointF(x,y);
            }
        }

        void Compute()
        {
            //变换
            Fourier.Forward(FFTBuffer);

            CirclePaths = CalculateCirclePaths(FFTBuffer);
            for (int i = 0; i < CircleNum; i++)
            {
                int j = i;
                if (i >= CircleNum / 2) j = FFTLength - CircleNum + i;
                radius[i] = MathF.Abs((float)(CirclePaths[j + 1][0] - CirclePaths[j][0]).Magnitude);
            }
        }

        private List<List<Complex>> CalculateCirclePaths(Complex[] x)
        {
            List<List<Complex>> pos = new List<List<Complex>>();
            List<Complex[]> factor = new List<Complex[]>();

            for (int i = 0; i < x.Length; i++)
            {
                factor.Add(new Complex[x.Length]);
                for (int t = 0; t < x.Length; t++)
                {
                    factor[i][t] = x[i] * Complex.Exp(new Complex(0, 2 * Math.PI * i * t / x.Length))/x.Length*10;
                }
            }

            List<Complex> currentPos = new List<Complex>();
            for (int t = 0; t < x.Length; t++)
            {
                currentPos.Add(Complex.Zero);
            }
            pos.Add(currentPos);

            for (int i = 0; i < x.Length; i++)
            {
                currentPos = new List<Complex>(x.Length);
                for (int t = 0; t < x.Length; t++)
                {
                    var c = pos.Last()[t];
                    currentPos.Add(c + factor[i][t]);
                }
                pos.Add(currentPos);
            }

            return pos;
        }

        int t = 0;
        private void OnTick(object? sender, EventArgs e)
        {
            if (t == FFTLength)
            {
                return;
            }
            for (int i = 0; i < CircleNum +1; i++)
            {
                int j = i;
                if (i >= CircleNum / 2) j = FFTLength - CircleNum + i;
                center[i] = new PointF((float)CirclePaths[j][t].Real, (float)CirclePaths[j][t].Imaginary);
            }
            Path.Add(center.Last());
            t++;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.TranslateTransform(ClientSize.Width/2, ClientSize.Height/2);
            for(int i = 0; i < radius.Length; i++)
            {
                e.Graphics.DrawEllipse(Pens.White, center[i].X - radius[i], center[i].Y - radius[i], radius[i] * 2, radius[i] * 2);
                e.Graphics.DrawLine(Pens.White, center[i], center[i + 1]);
            }
            //e.Graphics.DrawLines(Pens.Green, SamplePoints);
            if (Path.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Red, Path.ToArray());
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.Space)
            {
                Sample();
                Compute();
                timer1.Interval = 30;
                timer1.Tick += OnTick;
                timer1.Start();
            }
        }
    }
}
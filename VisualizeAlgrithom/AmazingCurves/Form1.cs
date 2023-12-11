namespace curve
{
    //【动画】用一个数学公式绘制无穷多种曲线
    //x = cos(a)+cos(b)/2+sin(c)/3
    //y = sin(a)+sin(b)/2+cos(c)/3

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
            DoubleBuffered = true;

            this.timer1.Interval = 100;
            this.timer1.Tick += Tick;
            this.timer1.Start();

            ie = iterate();
        }

        private void Tick(object? sender, EventArgs e)
        {
            ie.MoveNext();
            Draw();
        }

        IEnumerator<int> ie;
        IEnumerator<int> iterate()
        {
            for(int a = 1; a < 100; a++)
            {
                for(int b = 1; b < 100; b++)
                {
                    for(int c = 1; c < 100; c++)
                    {
                        mA = a;
                        mB = b;
                        mC = c;
                        yield return 0;
                    }
                }
            }
        }

        float mA = 1;
        float mB = 1;
        float mC = 1;

        float mR = 100;
        List<PointF> mPoints = new List<PointF>();
        void Draw()
        {
            mPoints.Clear();
            for (float i = 0; i < 360; i+=0.1f)
            {
                float t = (float)i / 180 * MathF.PI;
                float x = MathF.Cos(mA * t) + MathF.Cos(mB * t) / 2 + MathF.Sin(mC * t) / 3;
                float y = MathF.Sin(mA * t) + MathF.Sin(mB * t) / 2 + MathF.Cos(mC * t) / 3;
                mPoints.Add(new PointF(x*mR, y*mR)+ClientSize/2);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(mPoints.Count == 0) { return; }
            e.Graphics.DrawLines(Pens.YellowGreen, mPoints.ToArray());
        }
    }
}
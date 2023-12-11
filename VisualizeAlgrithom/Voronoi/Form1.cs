using System;

namespace Voronoi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        Random randor = new Random();
        //随机30个点R，和30种颜色C，遍历所有像素P，找到P[i]距离R最近的点R[j]，取C[j]作为P[i]的颜色
        const int N = 30;
        const int W = 1024;
        const int H = 768;
        Point[] R = new Point[N];
        Color[] C = new Color[N];
        Bitmap bmp = new Bitmap(W, H);
        void Init()
        {
            for (int i = 0; i < N; i++)
            {
                R[i] = new Point(randor.Next(W), randor.Next(H));
                C[i] = Color.FromArgb(randor.Next(255), randor.Next(255), randor.Next(255));
            }

            ClientSize = new Size(W, H);
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.timer1.Tick += OnTick;
            this.timer1.Interval = 30;
            this.timer1.Start();

        }
        private void OnTick(object? sender, EventArgs e)
        {
            for (int i = 0; i < N; i++)
            {
                R[i].X += randor.Next(10) - 5;
                R[i].Y += randor.Next(10) - 5;
            }

            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    float minDist = W * H;
                    Color color = Color.AliceBlue;
                    for (int i = 0; i < N; i++)
                    {
                        Point p = R[i];
                        float d = (p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y);
                        if (d < minDist)
                        {
                            minDist = d;
                            color = C[i];
                            //int cc = (int)minDist % 255;
                            //int cc = (int)(R[i].X + R[i].Y) % 255;
                            //color = Color.FromArgb(10, 100, cc);
                        }
                    }

                    bmp.SetPixel(x, y, color);
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
}
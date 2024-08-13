using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace RopeIK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();

            DoubleBuffered = true;
            BackColor = Color.Black;
            WindowState = FormWindowState.Maximized;

            timer1.Interval = 10;
            timer1.Tick += Tick;
            timer1.Start();

        }


        private void Tick(object sender, EventArgs e)
        {
            if (TargetPos.X > 0 && TargetPos.Y > 0)
            {
                Vector2 movev = TargetPos - circles[0].Center;
                if (movev.Length() < 10)
                {
                    TargetPos.X = 0;
                    TargetPos.Y = 0;
                }
                MoveOnce(TargetPos, 5);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            circles[0].Draw(g, null, Brushes.Red);
            for (int i = 1; i < circles.Count; i++)
            {
                circles[i].Draw(g, Pens.YellowGreen, null);
            }

            for (int i = 0; i < circles.Count - 1; i++)
            {
                g.DrawLine(Pens.BlueViolet, circles[i].Center.X, circles[i].Center.Y, circles[i + 1].Center.X, circles[i + 1].Center.Y);
            }

            g.FillEllipse(Brushes.GreenYellow, TargetPos.X - 5, TargetPos.Y - 5, 10, 10);

            float len = 2*Radius * Length - Radius;
            g.DrawEllipse(Pens.Wheat, FixPos.X - len, FixPos.Y - len, len*2, len*2);
        }

        List<Circle> circles = new List<Circle>();
        float Radius = 15;
        int Length = 10;
        Vector2 FixPos = new Vector2();
        Vector2 TargetPos = new Vector2();
        void Init()
        {
            for (int i = 0; i < Length; i++)
            {
                circles.Add(new Circle(new Vector2(1920 / 2, 1080 / 2), Radius));
            }
            FixPos = circles.Last().Center;

            MoveOnce(new Vector2(1920, 0), Radius*10);
        }

        void MoveOnce(Vector2 target, float step)
        {
            Vector2 v = target - circles[0].Center;
            v = Vector2.Normalize(v) * step;
            MoveForward(v);

            Vector2 u = FixPos - circles[circles.Count - 1].Center;
            MoveBack(u);

            Invalidate();
        }

        void MoveForward(Vector2 v)
        {
            circles[0].Center += v;
            for (int i = 1; i < circles.Count; i++)
            {
                Vector2 u = circles[i - 1].Center - circles[i].Center;
                circles[i].Center += u * 0.1f;//弹簧效果

                //位置约束，使绳子不能伸缩
                {
                    Vector2 d = circles[i].Center - circles[i - 1].Center;
                    circles[i].Center = circles[i - 1].Center + Vector2.Normalize(d) * circles[i - 1].Radius * 2;
                }

                //角度约束，使绳子更加平滑没有锐角
                if (i < circles.Count - 1)
                {
                    Vector2 p = circles[i - 1].Center - circles[i].Center;
                    Vector2 q = circles[i + 1].Center - circles[i].Center;
                    Vector2 leave = Vector2.Normalize(q - p) * circles[i].Radius;
                    float param = Vector2.Dot(p, q) / (p.Length() * q.Length());
                    param = (param + 1) / 2;//归一化到0-1
                    //param = (float)Math.Pow(param, 3);
                    circles[i + 1].Center += leave * param;

                    Vector2 d = circles[i + 1].Center - circles[i].Center;
                    circles[i + 1].Center = circles[i].Center + Vector2.Normalize(d) * circles[i].Radius * 2;

                }
            }
        }

        void MoveBack(Vector2 v)
        {
            circles[circles.Count - 1].Center += v;
            for (int i = circles.Count - 2; i >= 0; i--)
            {
                int prev = i + 1;
                int next = i - 1;
                Vector2 u = circles[prev].Center - circles[i].Center;
                circles[i].Center += u * 0.1f;//弹簧效果

                //位置约束，使绳子不能伸缩
                {
                    Vector2 d = circles[i].Center - circles[prev].Center;
                    circles[i].Center = circles[prev].Center + Vector2.Normalize(d) * circles[prev].Radius * 2;
                }

                //角度约束，使绳子更加平滑没有锐角
                if (i > 0)
                {
                    Vector2 p = circles[prev].Center - circles[i].Center;
                    Vector2 q = circles[next].Center - circles[i].Center;
                    Vector2 leave = Vector2.Normalize(q - p) * circles[i].Radius;
                    float param = Vector2.Dot(p, q) / (p.Length() * q.Length());
                    param = (param + 1) / 2;//归一化到0-1
                    //param = (float)Math.Pow(param, 3);
                    circles[next].Center += leave * param;

                    Vector2 d = circles[next].Center - circles[i].Center;
                    circles[next].Center = circles[i].Center + Vector2.Normalize(d) * circles[i].Radius * 2;

                }
            }

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                TargetPos = new Vector2(e.X, e.Y);
            }
        }
    }

    class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public void Draw(Graphics g, Pen pen, Brush brush)
        {
            if (pen != null)
            {
                g.DrawEllipse(Pens.LightYellow, (Center.X - Radius), (Center.Y - Radius), (Radius * 2), (Radius * 2));
            }
            g.FillEllipse(Brushes.Red, Center.X - 1, Center.Y - 1, 2, 2);
            if (brush != null)
            {
                g.FillEllipse(brush, Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);
            }
        }

        public bool InCircle(float x, float y)
        {
            Vector2 v = new Vector2(x, y);
            if (Vector2.Distance(v, Center) <= Radius)
            {
                return true;
            }
            return false;
        }


    }

}

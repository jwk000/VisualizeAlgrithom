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
using System.Reflection;

namespace simulate
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

        Random randor = new Random();
        List<Circle> circles = new List<Circle>();
        List<PointF> leftside = new List<PointF>();
        List<PointF> rightside = new List<PointF>();
        float Radius = 15;
        void Init()
        {
            for (int i = 0; i < 3; i++)
            {
                circles.Add(new Circle(new Vector2(Width / 2, Height / 2), Radius));
            }
            MoveOnce(new Vector2(0, -50));
        }

        //每隔5秒出现一个苹果
        long appleTimer = 5000;
        Circle apple = new Circle(new Vector2(-100,-100), 10);
        private void Tick(object sender, EventArgs e)
        {
            if(targetPos.X>0&& targetPos.Y > 0)
            {
                Vector2 movev = targetPos - circles[0].Center;
                if (movev.Length() < 10)
                {
                    targetPos.X = targetPos.Y = 0;
                    appleTimer += 5000;//重新生成苹果
                    //虫虫变长了
                    GrowOnce();
                }
                Vector2 step = Vector2.Normalize(movev) * 5;
                MoveOnce(step);
            }

            appleTimer += timer1.Interval;
            if (appleTimer > 5000)
            {
                //生成苹果
                appleTimer = 0;
                apple.Center = new Vector2(randor.Next(20,Width-20),randor.Next(20,Height-20));
                targetPos = apple.Center;
            }

            if (apple.Center.X > 0)
            {
                apple.Radius = (float)Math.Sin(appleTimer*0.01) * 10 + 5;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawApple(e.Graphics);

            DrawBody(e.Graphics);
        }

        void DrawApple(Graphics g)
        {
            apple.Draw(g,null,Brushes.LawnGreen);
        }

        void DrawBody(Graphics g)
        {
            circles[0].Draw(g, null, Brushes.Red);
            for(int i = 1; i < circles.Count; i++)
            {
                circles[i].Draw(g, Pens.YellowGreen, null);
            }

            for (int i = 0; i < circles.Count - 1; i++)
            {
                g.DrawLine(Pens.BlueViolet, circles[i].Center.X, circles[i].Center.Y, circles[i + 1].Center.X, circles[i + 1].Center.Y);
            }

            //计算边缘
            leftside.Clear();
            rightside.Clear();
            for (int i = 0; i < circles.Count - 1; i++)
            {
                Vector2 centerA = circles[i].Center;
                Vector2 centerB = circles[i + 1].Center;
                float radiusA = circles[i].Radius;
                float radiusB = circles[i + 1].Radius;

                // 计算中心点之间的向量 (dx, dy)
                float dx = centerB.X - centerA.X;
                float dy = centerB.Y - centerA.Y;

                // 计算向量的法线方向
                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                Vector2 normal = new Vector2(-dy / length, dx / length);

                // 计算A1和A2
                PointF A1 = new PointF(centerA.X + normal.X * radiusA, centerA.Y + normal.Y * radiusA);
                PointF A2 = new PointF(centerA.X - normal.X * radiusA, centerA.Y - normal.Y * radiusA);

                // 计算B1和B2
                PointF B1 = new PointF(centerB.X + normal.X * radiusB, centerB.Y + normal.Y * radiusB);
                PointF B2 = new PointF(centerB.X - normal.X * radiusB, centerB.Y - normal.Y * radiusB);
                if (i == 0)
                {
                    rightside.Add(A1);
                    leftside.Add(A2);
                }
                rightside.Add(B1);
                leftside.Add(B2);
            }

            rightside.Reverse();
            leftside.AddRange(rightside);
            var points = leftside;
            // 绘制贝塞尔曲线连接起来形成一个圈
            for (int i = 0; i <= points.Count-2; i+=2)
            {
                PointF prev = (i == 0) ?rightside.Last(): points[i - 1];
                PointF next = (i == points.Count - 2) ? leftside[0]: points[i + 2];
                PointF p0 =  new PointF((prev.X + points[i].X) / 2, (prev.Y + points[i].Y) / 2);
                PointF p1 = points[i];
                PointF p2 = points[i + 1];
                PointF p3 =  new PointF((points[i + 1].X +next.X) / 2, (points[i + 1].Y +next.Y) / 2);

                g.DrawBezier(Pens.Magenta, p0, p1, p2, p3);
                //g.DrawString(i.ToString(), new Font("Arial", 10), Brushes.White, p1);
            }
        }

        void GrowOnce()
        {
            circles.Add(new Circle(circles.Last().Center, Radius));
            //Radius += 0.5f;
            //foreach(var c in circles)
            //{
            //    c.Radius = Radius;
            //}
        }

        void MoveOnce(Vector2 v)
        {
            circles[0].Center += v;
            for (int i = 1; i < circles.Count; i++)
            {
                Vector2 u = circles[i - 1].Center - circles[i].Center;
                circles[i].Center += u * 0.1f;//弹簧效果

                //修正位置，使绳子不能伸缩
                FixLength(i);

                //使绳子更加平滑没有锐角
                if (i < circles.Count - 1)
                {
                    Vector2 p = circles[i - 1].Center - circles[i].Center;
                    Vector2 q = circles[i + 1].Center - circles[i].Center;
                    Vector2 leave = Vector2.Normalize(q - p)*circles[i].Radius;
                    float param = Vector2.Dot(p, q) / (p.Length() * q.Length());
                    param = (param + 1) / 2;//归一化到0-1
                    //param = (float)Math.Pow(param, 3);
                    circles[i + 1].Center += leave * param;
                    FixLength(i + 1);
                }
            }

            Invalidate();
        }

        void FixLength(int i)
        {
            //修正位置，使绳子不能伸缩
            Vector2 d = circles[i].Center - circles[i - 1].Center;
            circles[i].Center = circles[i - 1].Center + Vector2.Normalize(d) * circles[i - 1].Radius * 2;

        }

        bool holding = false;
        Vector2 targetPos = new Vector2(0, 0);
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (circles[0].InCircle(e.X, e.Y))
            {
                holding = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            holding = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (holding)
            {
                Vector2 movev = new Vector2(e.X - circles[0].Center.X, e.Y - circles[0].Center.Y);
                MoveOnce(movev);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            targetPos.X = e.X;
            targetPos.Y = e.Y;
        }
    }
}

using System.Numerics;

namespace RopeNet2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateNet();
            DoubleBuffered = true;
            ClientSize = new Size(800, 800);
            this.timer1.Interval = 10;
            this.timer1.Tick += OnTick;
            this.timer1.Start();
        }
        class Ball
        {
            public bool Pin;
            public Vector2 Pos;
            public float Radius;
            public Vector2 Force;
            public List<Link> Links = new List<Link>();
        }

        class Link
        {
            public Ball LinkBall;
            public float RestLength = 120;
        }
        List<Ball> mBalls = new List<Ball>();

        void CreateNet()
        {
            mBalls.Add(new Ball() { Pin = true, Pos = new Vector2(400, 400), Radius = 20 });
            for(int i = 0; i < 10; i++)
            {
                float t = i * MathF.PI * 2 / 10;
                float dx = MathF.Cos(t) * RestLen;
                float dy = MathF.Sin(t) * RestLen;
                var b = new Ball() { Pin = false, Pos = new Vector2(400 + dx, 400 + dy), Radius = 20 };
                mBalls.Add(b);
                b.Links.Add(new Link() { LinkBall = mBalls[0] });
            }
        }

        const float NormalForce = 3000;
        const float MaxForce = 5000;
        const float MinForce = 1000;
        const float Step = 0.01f;
        const float RestLen = 200;

        private void OnTick(object? sender, EventArgs e)
        {
            //计算所有球的斥力
            foreach (Ball b in mBalls)
            {
                if (b.Pin)
                {
                    continue;
                }
                b.Force = Vector2.Zero;
                foreach (Ball x in mBalls)
                {
                    if (x == b) { continue; }
                    Vector2 v = b.Pos - x.Pos;
                    float dist = MathF.Max(0.1f, v.Length() - b.Radius * 2);
                    b.Force += Vector2.Normalize(v) * MaxForce / (dist*dist);
                }

                if (b.Force.Length() < MinForce)
                {
                    b.Force = MinForce * Vector2.Normalize(b.Force);
                }
                else if (b.Force.Length() > MaxForce)
                {
                    b.Force = MaxForce* Vector2.Normalize(b.Force);
                }
            }

            //move one step
            foreach (Ball b in mBalls)
            {
                b.Pos += b.Force * Step;
                //fix pos
                if(Vector2.Distance( b.Pos , mBalls[0].Pos) > RestLen)
                {
                    b.Pos = mBalls[0].Pos + Vector2.Normalize(b.Pos - mBalls[0].Pos) * RestLen;
                }
            }
            Invalidate();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            foreach (Ball b in mBalls)
            {
                g.FillEllipse(Brushes.Red, new RectangleF(new PointF(b.Pos.X - 2, b.Pos.Y - 2), new Size(5, 5)));
                g.DrawEllipse(Pens.Black, new RectangleF(new PointF(b.Pos.X - b.Radius, b.Pos.Y - b.Radius), new SizeF(2 * b.Radius, 2 * b.Radius)));
                foreach (Link l in b.Links)
                {
                    g.DrawLine(Pens.Gray, new PointF(b.Pos), new PointF(l.LinkBall.Pos));
                }
            }


        }

        int mDragState = 0;//0未拖拽 1拖拽中 

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Ball ball = mBalls[0];
            Vector2 ep = new Vector2(e.X, e.Y);
            Vector2 me = ep - ball.Pos;
            if (me.Length() < ball.Radius)
            {
                mDragState = 1;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (mDragState == 1) { mDragState = 0; }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (mDragState == 1)
            {
                mBalls[0].Pos = new Vector2(e.X, e.Y);
            }
        }
    }
}

using System.Numerics;

namespace Rope
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        MassSpringSystem mssystem = new MassSpringSystem();
        //����ģ��
        int mDragState = 0;//0δ��ק 1��ק�� 

        void Init()
        {
            mssystem.MakeRope();

            DoubleBuffered = true;
            ClientSize = new Size(800, 800);
            this.timer1.Interval = 10;
            this.timer1.Tick += OnTick;
            this.timer1.Start();
        }
        private void OnTick(object? sender, EventArgs e)
        {
            mssystem.Update(0.1f);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            List<PointF> pts = mssystem.GetPoints().Select(p => new PointF(p.X, p.Y)).ToList();
            g.DrawLines(Pens.Black, pts.ToArray());
            foreach (PointF p in pts)
            {
                g.FillEllipse(Brushes.Red, new RectangleF(new PointF(p.X - 2, p.Y - 2), new Size(5, 5)));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MassPoint mp = mssystem.massPoints[0];
            Vector2 ep = new Vector2(e.X, e.Y);
            Vector2 me = ep - mp.position;
            if (me.Length() < 10)
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
                mssystem.massPoints[0].position = new Vector2(e.X, e.Y);
                mssystem.Update(0.1f);
            }
        }
    }
}
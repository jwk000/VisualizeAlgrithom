using System.Numerics;

namespace Spirograph
{
        /// <summary>
        /// 繁花曲线：大圆套小圆，小圆绕着大圆转
        /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
            DoubleBuffered = true;
            ClientSize = new Size(800, 800);
            InitSpirograph();
            this.timer1.Interval = 30;
            this.timer1.Tick += OnTick;
        }


        float R = 211;//大圆
        float r = 37;//小圆
        float t = MathF.PI / 180;//大圆速度
        Vector2 mCircleCenter;//小圆的圆心
        List<Vector2> mPinPts = new List<Vector2>();//控制点
        List<PointF> mLastPts=new List<PointF>();//历史位置
        Bitmap bmp;
        Graphics g_bmp;

        void InitSpirograph()
        {
            
            mCircleCenter = new Vector2(R - r, 0);
            mLastPts.Clear();
            mPinPts.Clear();
            mPinPts.Add(mCircleCenter);
            mLastPts.Add(new PointF(mCircleCenter));
            bmp = new Bitmap(800, 800);
            g_bmp = Graphics.FromImage(bmp);
            g_bmp.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            Invalidate();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            var oldCenter = mCircleCenter;
            mCircleCenter = Vector2.Transform(mCircleCenter, Matrix3x2.CreateRotation(t));
            for (int i = 0; i < mPinPts.Count; i++)
            {
                //移动到原点，旋转，移动到目标点
                Vector2 pt = mPinPts[i];
                pt = pt - oldCenter;
                pt = Vector2.Transform(pt, Matrix3x2.CreateRotation(-t * R / r));
                mPinPts[i] = pt + mCircleCenter;
                PointF p = new PointF(mPinPts[i]);
                g_bmp.DrawLine(Pens.Yellow, mLastPts[i], p);
                mLastPts[i] = p;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            base.OnPaint(e);

            e.Graphics.DrawImage(bmp, 0, 0);

            e.Graphics.TranslateTransform(ClientSize.Width / 2, ClientSize.Height / 2);
            //大圆
            DrawCircle(e.Graphics, 0, 0, R);
            //小圆
            DrawCircle(e.Graphics, mCircleCenter.X, mCircleCenter.Y, r);
            //圆上的绘制点
            foreach (var pt in mPinPts)
            {
                e.Graphics.DrawLine(Pens.Gray, new PointF(pt), new PointF(mCircleCenter));
                FillCircle(e.Graphics, pt.X, pt.Y, 3);
            }
        }

        void DrawCircle(Graphics g, float x, float y, float r)
        {
            g.DrawEllipse(Pens.Gray, x - r, y - r, r + r, r + r);
        }
        void FillCircle(Graphics g, float x, float y, float r)
        {
            g.FillEllipse(Brushes.Red, x - r, y - r, r + r, r + r);
        }

        //拖动小圆圆心
        int selectIdx = -1;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Vector2 p = new Vector2(e.X - ClientSize.Width / 2, e.Y - ClientSize.Height / 2);
            for (int i = 0; i < mPinPts.Count; i++)
            {
                var pt = mPinPts[i];
                if (Vector2.Distance(p, pt) < 5)
                {
                    selectIdx = i;
                    //按下ctrl复制
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        mPinPts.Add(pt);
                        mLastPts.Add(new PointF(pt));
                        selectIdx = mPinPts.Count - 1;
                    }
                    break;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            selectIdx = -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Vector2 p = new Vector2(e.X - ClientSize.Width / 2, e.Y - ClientSize.Height / 2);
            if (selectIdx >= 0)
            {
                mPinPts[selectIdx] = p;
                mLastPts[selectIdx] = new PointF(p);
            }
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                this.timer1.Start();
            }
            if(e.KeyCode == Keys.Escape)
            {
                this.timer1.Stop();
                InitSpirograph();
            }
        }
    }
}
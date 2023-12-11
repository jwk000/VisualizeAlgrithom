using System.Numerics;

namespace BezierCurve
{
    //左键点击选择控制点，右键确认绘制贝赛尔曲线
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //贝塞尔曲线控制点
        List<Point> mBezierCtrlPts = new List<Point>();
        Action<Graphics> mOnPaint;
        bool mConnectiing = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            mOnPaint?.Invoke(e.Graphics);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mConnectiing = true;
                mBezierCtrlPts.Add(e.Location);
                if (mBezierCtrlPts.Count > 1)
                {
                    mOnPaint = g => g.DrawLines(Pens.Black, mBezierCtrlPts.ToArray());
                    Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                mConnectiing = false;
                BezierCurve bc = new BezierCurve();
                mOnPaint = g =>
                {
                    g.DrawLines(Pens.Black, mBezierCtrlPts.ToArray());

                    List<Vector2> ctrls = mBezierCtrlPts.Select(p => new Vector2(p.X, p.Y)).ToList();
                    List<PointF> points = bc.DrawBezier(ctrls).Select(v => new PointF(v.X, v.Y)).ToList();
                    g.DrawLines(Pens.IndianRed, points.ToArray());

                    //g.DrawBeziers(Pens.Green, mBezierCtrlPts.ToArray());
                    mBezierCtrlPts.Clear();
                };
                Invalidate();

            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!mConnectiing) return;
            List<Point> pts = new List<Point>();
            if (mBezierCtrlPts.Count > 0)
            {
                pts.AddRange(mBezierCtrlPts);
                pts.Add(e.Location);
                mOnPaint = g => g.DrawLines(Pens.Black, pts.ToArray());
                Invalidate();
            }

        }
    }
}
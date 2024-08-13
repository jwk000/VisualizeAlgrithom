using System.Numerics;

namespace CatmullRom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;
            DoubleBuffered = true;

        }
        Pen catmallPen = new Pen(Color.Green, 3);
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mOutputs.Count > 2)
            {
                e.Graphics.DrawLines(catmallPen, mOutputs.ToArray());
            }
            if (mPoints.Count > 2)
            {
                e.Graphics.DrawCurve(Pens.Blue, mPoints.ToArray());
            }
            foreach (var point in mPoints)
            {
                e.Graphics.FillEllipse(Brushes.Red, point.X - 2, point.Y - 2, 4, 4);
            }
        }

        List<PointF> mPoints = new List<PointF>();
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                mPoints.Add(new PointF(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {
                CalcCatmullRom();
            }

            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                mPoints.Clear();
                mOutputs.Clear();
                Invalidate();
            }
        }


        List<PointF> mOutputs = new List<PointF>();

        //公式推导 https://www.cnblogs.com/autodriver/p/18086046
        List<PointF> CatmullRom(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3,  float k)
        {
            float step = MathF.Abs( 1.0f/(P1.X-P2.X));
            List<PointF> output = new List<PointF>();
            for (float t = 0; t <= 1; t += step)
            {

                Vector2 C = 
                    P0*(-k*t*t*t+2*k*t*t-k*t)+
                    P1*((2-k)*t*t*t+(k-3)*t*t+1)+
                    P2*((k-2)*t*t*t+(3-2*k)*t*t+k*t)+
                    P3*(k*t*t*t-k*t*t);

                output.Add(new PointF(C));
            }
            return output;
        }

        void CalcCatmullRom()
        {
            mOutputs.Clear();
            List<Vector2> vs = new List<Vector2>();
            vs.Add(new Vector2(mPoints[0].X, mPoints[0].Y));
            foreach(var point in mPoints)
            {
                vs.Add(new Vector2(point.X, point.Y));
            }
            PointF lastone = mPoints[mPoints.Count - 1];
            vs.Add(new Vector2(lastone.X, lastone.Y));

            for (int i = 0; i + 3 < vs.Count; i++)
            {
                Vector2 P0 = vs[i];
                Vector2 P1 = vs[i + 1];
                Vector2 P2 = vs[i + 2];
                Vector2 P3 = vs[i + 3];
                mOutputs.AddRange(CatmullRom(P0, P1, P2, P3, 0.5f));//k=0.5的时候和drawcurve()结果一致
            }
        }
    }
}

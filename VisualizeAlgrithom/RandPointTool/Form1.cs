using System.Diagnostics;
using System.Numerics;

namespace RandPointTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            BackColor = Color.White;
            WindowState = FormWindowState.Maximized;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawLine(Pens.Black, 0, 100, ClientSize.Width - 1, 100);
            e.Graphics.DrawLine(Pens.Black, 0, 100, 0, ClientSize.Height - 1);
            e.Graphics.DrawLine(Pens.Black, 0, ClientSize.Height - 1, ClientSize.Width - 1, ClientSize.Height - 1);
            e.Graphics.DrawLine(Pens.Black, ClientSize.Width - 1, 100, ClientSize.Width - 1, ClientSize.Height - 1);

            foreach (PointF p in mPoints)
            {
                e.Graphics.FillEllipse(Brushes.Red, p.X * mScale - 2, p.Y * mScale + 100 - 2, 4, 4);
            }

            e.Graphics.DrawEllipse(Pens.Black,
                mScale * mCenterPos.X - mScale * mRadius,
                mScale * mCenterPos.Y - mScale * mRadius + 100,
                mScale * mRadius * 2,
                mScale * mRadius * 2);
        }

        Random mRandor = new Random();
        //米切尔采样
        //（1）全屏随机一个点p，找到距离p最近的已采样点q，p和q的距离=d
        //（2）重复第一步k次，把d最大的点加入采样结果
        public List<PointF> MichellSample(int width, int height, int num)
        {
            List<PointF> ret = new List<PointF>();
            List<PointF> tmp = new List<PointF>();

            //第一个点
            tmp.Add(new PointF(mRandor.Next(1, width), mRandor.Next(1, height)));
            if (!InCircle(tmp[0]))
            {
                ret.Add(tmp[0]);
                num--;
            }
            while (num > 0)
            {
                PointF p = new PointF();
                float distance = 0;
                for (int n = 0; n < 10; n++)
                {
                    PointF c = new PointF(mRandor.Next(1, width), mRandor.Next(1, height));

                    float d = FindClosest(tmp, c);
                    if (d > distance)
                    {
                        distance = d;
                        p = c;
                    }
                }
                tmp.Add(p);
                if (!InCircle(p))
                {
                    ret.Add(p);
                    num--;
                }
            }
            return ret;
        }

        bool InCircle(PointF point)
        {
            float x = point.X - mCenterPos.X;
            float y = point.Y - mCenterPos.Y;
            return x * x + y * y < mRadius * mRadius;
        }

        float FindClosest(List<PointF> samples, PointF p)
        {
            float distance = 1000000;
            foreach (PointF q in samples)
            {
                float x = p.X - q.X;
                float y = p.Y - q.Y;
                float d = x * x + y * y;
                if (d < distance)
                {
                    distance = d;
                }
            }
            return distance;
        }

        List<PointF> mPoints = new List<PointF>();
        Vector3 mStartPos;
        PointF mCenterPos;
        int mRadius = 0;
        float mScale = 1;
        private void btnGen_Click(object sender, EventArgs e)
        {
            string p = txtLeftBottom.Text;
            string[] pp = p.Split(',');
            if (pp.Length != 3)
            {
                MessageBox.Show("左下坐标格式: x,y,z ");
                return;
            }
            int x1 = int.Parse(pp[0]);
            int y1 = int.Parse(pp[1]);
            int z1 = int.Parse(pp[2]);
            mStartPos = new Vector3(x1, y1, z1);

            string q = txtRightUp.Text;
            string[] qq = q.Split(',');
            if (qq.Length != 3)
            {
                MessageBox.Show("右上坐标格式: x,y,z ");
                return;
            }
            int x2 = int.Parse(qq[0]);
            int y2 = int.Parse(qq[1]);
            int z2 = int.Parse(qq[2]);
            //Vector3 v2 = new Vector3(x2, y2, z2);

            string w = txtCircle.Text;
            string[] ww = w.Split(',');
            if (ww.Length != 3)
            {
                MessageBox.Show("圆心坐标格式: x,y,z ");
                return;
            }
            int x3 = int.Parse(ww[0]);
            int y3 = int.Parse(ww[1]);
            int z3 = int.Parse(ww[2]);

            mCenterPos.X = x3 - x1;
            mCenterPos.Y = y3 - y1;

            mRadius = int.Parse(txtRadius.Text);
            if (mRadius < 0)
            {
                MessageBox.Show("圆的半径不能<0");
                return;
            }

            int width = x2 - x1;
            int height = y2 - y1;
            mScale = ClientSize.Width * 1.0f / width;
            if (width <= 0 || height <= 0)
            {
                MessageBox.Show("右上坐标应该大于左下坐标");
                return;
            }

            if (z1 != z2 || z1 != z3)
            {
                MessageBox.Show("左下，右上，圆心坐标的z值必须一致");
                return;
            }


            int num = int.Parse(txtNum.Text);
            if (num <= 0)
            {
                MessageBox.Show("种怪数量必须大于0");
                return;
            }
            mPoints = MichellSample(width, height, num);

            Invalidate();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("points.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var p in mPoints)
                    {
                        sw.WriteLine("{0},{1},{2}", p.X + mStartPos.X, p.Y + mStartPos.Y, mStartPos.Z);
                    }
                }
            }

            Process proc = new Process();
            proc.StartInfo.FileName = "Code.exe";
            proc.StartInfo.Arguments = "points.txt";
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
        }
    }
}

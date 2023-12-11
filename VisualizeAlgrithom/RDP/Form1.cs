using System.Numerics;

namespace RDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (mSelected.Count >= 2)
            {
                e.Graphics.DrawLines(Pens.White, mSelected.Select(v => new PointF(v.X, v.Y)).ToArray());
            }
            if (mSimilar.Count >= 2)
            {
                e.Graphics.DrawLines(Pens.Red, mSimilar.Select(v => new PointF(v.X, v.Y)).ToArray());
            }
        }

        List<Vector2> mSelected = new List<Vector2>();
        List<Vector2> mSimilar = new List<Vector2>();
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                mSelected.Add(new Vector2(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Right)
            {
                mSimilar = RDP.Similar(mSelected, 10);
            }
            Invalidate();
        }

    }


    class RDP
    {
        //rdp����ƽ���㷨
        //�������ߵ㼯��pts[]��ѡ�����s���յ�e��һ��ֱ��L��Ѱ��pts�����L��Զ�ĵ�q������d��
        //�������d<��С���̾���r����L�����еĵ㶼���Զ�����
        //������s,q��q,e��ֱ�ߣ��ݹ����ϲ��裬ֱ��û�е���Զ�����

        public static List<Vector2> Similar(List<Vector2> pts, float r)
        {
            if (pts == null || pts.Count <= 2) return pts;
            bool[] kept = new bool[pts.Count];
            rdp(pts, kept, 0, pts.Count - 1, r);
            List<Vector2> ans = new List<Vector2>();
            for (int i = 0; i < pts.Count; i++)
            {
                if (kept[i]) ans.Add(pts[i]);
            }
            return ans;
        }

        static void rdp(List<Vector2> pts, bool[] kept, int s, int e, float r)
        {
            if (s >= e) return;
            kept[s] = kept[e] = true;
            Vector2 line = Vector2.Normalize(pts[e] - pts[s]);
            Vector2 normal = new Vector2(line.Y, -line.X);//����
            float maxDist = 0;
            int q = s;
            for (int i = s + 1; i < e; i++)
            {
                float d = MathF.Abs(Vector2.Dot(normal, pts[i] - pts[s]));
                if (d > maxDist)
                {
                    maxDist = d;
                    q = i;
                }
            }
            if (maxDist >= r)
            {
                rdp(pts, kept, s, q, r);
                rdp(pts, kept, q, e, r);
            }
        }
    }

}
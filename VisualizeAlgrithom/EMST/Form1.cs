using System.Drawing.Printing;
using System;
using System.Numerics;

namespace MininumSpanningTree
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
            ClientSize = new Size(1000, 1000);

            Init(false);
        }

        List<Vector2> mPts = new List<Vector2>();
        List<Edge> mEdges = new List<Edge>();
        const int nPCount = 20;
        const int nMargin = 200;
        Random randor = new Random();
        IEnumerator<List<Edge>> mEnum;
        bool bDebug = false;
        void Init(bool debug)
        {
            mPts.Clear();
            for (int i = 0; i < nPCount; i++)
            {
                mPts.Add(new Vector2(randor.Next(nMargin, ClientSize.Width - nMargin), randor.Next(nMargin, ClientSize.Height - nMargin)));
            }
            mEdges.Clear();
            mEnum = null;
            bDebug = debug;

            if(debug)mEnum = EMST.GenLive(mPts);
            else mEdges = EMST.Gen(mPts);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PointF[] pts = new PointF[mPts.Count];

            for (int i = 0; i < mPts.Count; i++)
            {
                pts[i] = new PointF(mPts[i].X, mPts[i].Y);
                e.Graphics.DrawEllipse(Pens.Red, pts[i].X, pts[i].Y, 3, 3);
                e.Graphics.DrawString(i.ToString(), SystemFonts.DefaultFont, Brushes.GreenYellow, pts[i]);
            }
            foreach (Edge ed in mEdges)
            {
                e.Graphics.DrawLine(Pens.Yellow, pts[ed.a], pts[ed.b]);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.F10 && bDebug)
            {
                if (mEnum.MoveNext())
                {
                    mEdges = mEnum.Current;
                }
                else
                {
                    bDebug = false;
                }
                Invalidate();
            }
            if(e.KeyCode == Keys.Escape)
            {
                Init(false);
            }
            if(e.KeyCode == Keys.F5)
            {
                Init(true);
            }
        }
    }

    //Euclidean Minimum Spanning Tree
    //欧几里得平面空间最小生成树
    class Edge
    {
        public int a;
        public int b;
        public float w;
        public Edge(int a, int b, float w)
        {
            this.a = a;
            this.b = b;
            this.w = w;
        }
    }
    class EMST
    {
        public static List<Edge> Gen(List<Vector2> pts)
        {
            List<Edge> edges = new List<Edge>();
            List<Edge> all = new List<Edge>();
            int[] adj = new int[pts.Count];
            for (int i = 0; i < pts.Count; i++) adj[i] = -1;

            for (int i = 0; i < pts.Count; i++)
            {
                for (int j = i + 1; j < pts.Count; j++)
                {
                    float w = Vector2.DistanceSquared(pts[i], pts[j]);
                    all.Add(new Edge(i, j, w));
                }
            }

            all.Sort((e1, e2) => (int)(e1.w - e2.w));
            foreach (Edge e in all)
            {
                if (adj[e.a] != -1 && adj[e.a] == adj[e.b]) continue;
                if (adj[e.a] == -1) { adj[e.a] = e.a; }
                if (adj[e.b] == -1) { adj[e.b] = e.b; }
                int m = Math.Min(adj[e.a], adj[e.b]);
                int n = Math.Max(adj[e.a], adj[e.b]);
                for (int i = 0; i < adj.Length; i++)
                {
                    if (adj[i] == n) { adj[i] = m; }
                }
                edges.Add(e);
            }
            return edges;
        }


        public static IEnumerator<List<Edge>> GenLive(List<Vector2> pts)
        {
            List<Edge> edges = new List<Edge>();
            List<Edge> all = new List<Edge>();
            int[] adj = new int[pts.Count];
            for (int i = 0; i < pts.Count; i++) adj[i] = -1;

            for (int i = 0; i < pts.Count; i++)
            {
                for (int j = i + 1; j < pts.Count; j++)
                {
                    float w = Vector2.DistanceSquared(pts[i], pts[j]);
                    all.Add(new Edge(i, j, w));
                }
            }

            all.Sort((e1, e2) => (int)(e1.w - e2.w));
            foreach (Edge e in all)
            {
                if (adj[e.a] != -1 && adj[e.a] == adj[e.b]) continue;
                if (adj[e.a] == -1) { adj[e.a] = e.a; }
                if (adj[e.b] == -1) { adj[e.b] = e.b; }
                int m = Math.Min(adj[e.a], adj[e.b]);
                int n = Math.Max(adj[e.a], adj[e.b]);
                for(int i=0;i<adj.Length;i++)
                {
                    if (adj[i]==n) { adj[i] = m; }
                }
                edges.Add(e);
                yield return edges;
            }
            yield return edges;
        }
    }

}
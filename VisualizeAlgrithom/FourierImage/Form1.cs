using System.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace fourier2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            BackColor = Color.Black;
            ClientSize = new Size(1920, 1080);

        }

        Bitmap mBitmap;
        Bitmap mBinaryImage;
        Bitmap mEdgeImage;

        List<Vector2> mSampleVertexs = new List<Vector2>();
        List<Edge> mDelaunayEdges;     
        List<Vector2> mSMTPath = new List<Vector2>();
        List<Vector2> mRdpPath = new List<Vector2>();
        List<PointF> mFFTPath = new List<PointF>();
        List<List<Complex>> mCirclePaths;
        int mFFTLength;
        float mFFTScaler = 32;
        Complex[] mFFTBuffer;
        PointF[] mCircleCenter;
        Vector2[] mCenterPath;
        float[] mRadius;


        void HandleImage()
        {
            Environment.CurrentDirectory = Environment.CurrentDirectory + "/../../../";
            mBitmap = new Bitmap("zx2.jpg");
            // 图片二值化
            mBinaryImage = new Bitmap(mBitmap.Width, mBitmap.Height);
            for (int y = 0; y < mBitmap.Height; y++)
            {
                for (int x = 0; x < mBitmap.Width; x++)
                {
                    Color originalColor = mBitmap.GetPixel(x, y);
                    //颜色转换为灰度值
                    int grayScale = (int)(originalColor.R * 0.3 + originalColor.G * 0.59 + originalColor.B * 0.11);
                    //灰度值超过200视为白色，否则为黑色
                    Color newColor = grayScale > 200 ? Color.White : Color.Black;
                    mBinaryImage.SetPixel(x, y, newColor);
                }
            }
            // 边缘提取
            mEdgeImage = new Bitmap(mBinaryImage.Width, mBinaryImage.Height);
            for (int y = 1; y < mBinaryImage.Height - 1; y++)
            {
                for (int x = 1; x < mBinaryImage.Width - 1; x++)
                {
                    //判断每个像素上下、左右的颜色差值，如果差值超过50视为边缘
                    int gx = (mBinaryImage.GetPixel(x + 1, y).R - mBinaryImage.GetPixel(x - 1, y).R) / 2;
                    int gy = (mBinaryImage.GetPixel(x, y + 1).R - mBinaryImage.GetPixel(x, y - 1).R) / 2;
                    int gradient = Math.Abs(gx) + Math.Abs(gy);
                    if (mBinaryImage.GetPixel(x, y).R > 160 && gradient > 50)
                    {
                        mSampleVertexs.Add(new Vector2(x, y));//记录边缘像素
                        mEdgeImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        mEdgeImage.SetPixel(x, y, Color.Black);
                    }
                }
            }
            Invalidate();
        }
        Bitmap ApplyBinarization(Bitmap inputImage, int threshold)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    Color originalColor = inputImage.GetPixel(x, y);
                    int grayScale = (int)(originalColor.R * 0.3 + originalColor.G * 0.59 + originalColor.B * 0.11);

                    Color newColor = grayScale > threshold ? Color.White : Color.Black;

                    outputImage.SetPixel(x, y, newColor);
                }
            }

            return outputImage;
        }

        Bitmap ApplyEdgeDetection(Bitmap inputImage)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);

            for (int y = 1; y < inputImage.Height - 1; y++)
            {
                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    int gx = (inputImage.GetPixel(x + 1, y).R - inputImage.GetPixel(x - 1, y).R) / 2;
                    int gy = (inputImage.GetPixel(x, y + 1).R - inputImage.GetPixel(x, y - 1).R) / 2;

                    int gradient = Math.Abs(gx) + Math.Abs(gy);


                    if (inputImage.GetPixel(x, y).R > 160 && gradient > 50)
                    {
                        mSampleVertexs.Add(new Vector2(x, y));
                        outputImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        outputImage.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return outputImage;
        }

        void GenTriangles()
        {
            Delaunay delaunay = new Delaunay();
            mDelaunayEdges = delaunay.Triangulate(mSampleVertexs);
            mDelaunayEdges.Sort((e1, e2) => (int)(e1.w - e2.w));
        }

        List<Edge> mFullEdge = new List<Edge>();
        void GenFullEdge()
        {
            List<Vector2> pts = mSampleVertexs;
            for (int i = 0; i < pts.Count; i++)
            {
                for (int j = i + 1; j < pts.Count; j++)
                {
                    float w = Vector2.DistanceSquared(pts[i], pts[j]);
                    mFullEdge.Add(new Edge(i, j, w));
                }
            }
        }

        void GenSMTPath(List<Edge> edges)
        {
            List<Vector2> pts = mSampleVertexs;
            List<SMTNode> smt = new List<SMTNode>();

            int[] adj = new int[pts.Count];
            for (int i = 0; i < pts.Count; i++)
            {
                adj[i] = -1;
                smt.Add(new SMTNode(i));
            }

            foreach (Edge e in edges)
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
                smt[e.a].nodes.Add(e.b);
                smt[e.b].nodes.Add(e.a);
            }

            bool[] visited = new bool[smt.Count];
            mSMTPath.Clear();
            Action<SMTNode> dfs = null;
            dfs = (SMTNode node) =>
            {
                if (visited[node.index]) return;
                visited[node.index] = true;
                mSMTPath.Add(pts[node.index]);
                foreach (int j in node.nodes)
                {
                    dfs(smt[j]);
                    mSMTPath.Add(pts[node.index]);
                }
            };

            dfs(smt[0]);
        }


        void GenRdpPath()
        {
            mRdpPath.Clear();
            var part1 = Similar(mSMTPath.Take(mSMTPath.Count / 2).ToList(), 0.5f);
            var part2 = Similar(mSMTPath.Skip(mSMTPath.Count / 2).ToList(), 0.5f);
            mRdpPath.AddRange(part1);
            mRdpPath.AddRange(part2);
        }


        //rdp曲线平滑算法
        //给定曲线点集合pts[]，选择起点s和终点e连一条直线L，寻找pts里距离L最远的点q，距离d；
        //如果距离d<最小容忍距离r，则L上所有的点都可以丢弃；
        //否则以s,q和q,e作直线，递归以上步骤，直到没有点可以丢弃；

        List<Vector2> Similar(List<Vector2> pts, float r)
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

        void rdp(List<Vector2> pts, bool[] kept, int s, int e, float r)
        {
            if (s >= e) return;
            kept[s] = kept[e] = true;
            Vector2 line = Vector2.Normalize(pts[e] - pts[s]);
            Vector2 normal = new Vector2(line.Y, -line.X);//法线
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

        void HandleFFT(List<Vector2> path)
        {
            mFFTLength = path.Count;
            mFFTBuffer = new Complex[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                var v = path[i];
                mFFTBuffer[i] = new Complex(v.X, v.Y);
            }

            //变换
            Fourier.Forward(mFFTBuffer);
            //逆变换
            mCirclePaths = CalculateCirclePaths(mFFTBuffer);

            mRadius = new float[mFFTLength];
            mCircleCenter = new PointF[mFFTLength + 1];
            mCenterPath = new Vector2[mFFTLength + 1];
            for (int i = 0; i < mFFTLength; i++)
            {
                mRadius[i] = MathF.Abs((float)(mCirclePaths[i + 1][0] - mCirclePaths[i][0]).Magnitude);
            }

        }

        List<List<Complex>> CalculateCirclePaths(Complex[] x)
        {
            List<List<Complex>> pos = new List<List<Complex>>();
            List<Complex[]> factor = new List<Complex[]>();

            for (int i = 0; i < x.Length; i++)
            {
                factor.Add(new Complex[x.Length]);
                for (int t = 0; t < x.Length; t++)
                {
                    factor[i][t] = x[i] * Complex.Exp(new Complex(0, 2 * Math.PI * i * t / x.Length)) / x.Length * mFFTScaler;
                }
            }

            List<Complex> currentPos = new List<Complex>();
            for (int t = 0; t < x.Length; t++)
            {
                currentPos.Add(Complex.Zero);
            }
            pos.Add(currentPos);

            for (int i = 0; i < x.Length; i++)
            {
                currentPos = new List<Complex>(x.Length);
                for (int t = 0; t < x.Length; t++)
                {
                    var c = pos.Last()[t];
                    currentPos.Add(c + factor[i][t]);
                }
                pos.Add(currentPos);
            }

            return pos;
        }

        int t = 0;
        private void OnTick(object? sender, EventArgs e)
        {
            if (t == mFFTLength)
            {
                return;
            }
            for (int i = 0; i < mFFTLength + 1; i++)
            {
                mCenterPath[i] = new Vector2((float)mCirclePaths[i][t].Real, (float)mCirclePaths[i][t].Imaginary);
                mCircleCenter[i] = new PointF((float)mCirclePaths[i][t].Real, (float)mCirclePaths[i][t].Imaginary);
            }
            mFFTPath.Add(mCircleCenter.Last());
            t++;
            if (t == mFFTLength) { step++; }
            Invalidate();
        }

        int step = -1;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space) //空格单步调试
            {
                step++;
                if (step == 0) HandleImage();
                if (step == 3) GenTriangles();
                if (step == 4) GenSMTPath(mDelaunayEdges);
                if (step == 5) GenRdpPath();
                if (step == 6)
                {
                    HandleFFT(mRdpPath);
                    timer1.Interval = 100;
                    timer1.Tick += OnTick;
                    timer1.Start();
                }
                Invalidate();
            }
            else if(e.KeyCode == Keys.Enter) //回车一键画图
            {
                HandleImage();
                //GenFullEdge();
                GenTriangles();
                GenSMTPath(mDelaunayEdges);
                GenRdpPath();
                HandleFFT(mRdpPath);
                timer1.Interval = 100;
                timer1.Tick += OnTick;
                timer1.Start();
                step = 6;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TranslateTransform(ClientSize.Width / 3, ClientSize.Height / 3);
            if (step == 0)
            {
                e.Graphics.DrawImage(mBitmap, 0, 0);
            }
            if (step == 1)
            {
                e.Graphics.DrawImage(mBinaryImage, 0, 0);
            }
            if (step == 2)
            {
                e.Graphics.DrawImage(mEdgeImage, 0, 0);
            }
            if(step == 3 )
            {
                foreach (var edge in mDelaunayEdges)
                {
                    var a = mSampleVertexs[edge.a];
                    var b = mSampleVertexs[edge.b];
                    e.Graphics.DrawLine(Pens.LawnGreen, new PointF(a.X, a.Y), new PointF(b.X, b.Y));
                }
            }
            if (step == 4)
            {
                PointF[] pts = new PointF[mSMTPath.Count];
                for (int i = 0; i < mSMTPath.Count; i++)
                {
                    var v = mSMTPath[i];
                    pts[i] = new PointF(v.X, v.Y);
                }
                e.Graphics.DrawLines(Pens.LawnGreen, pts);
            }
            if (step == 5)
            {
                PointF[] pts = new PointF[mRdpPath.Count];
                for (int i = 0; i < mRdpPath.Count; i++)
                {
                    var v = mRdpPath[i];
                    pts[i]=new PointF(v.X, v.Y);
                }
                e.Graphics.DrawLines(Pens.LawnGreen, pts);
            }
            if (step == 6)
            {
                //e.Graphics.TranslateTransform(mCircleCenter.Last().X, mCircleCenter.Last().Y);
                for (int i = 0; i < mFFTLength; i++)
                {
                    e.Graphics.DrawEllipse(Pens.DarkCyan, mCircleCenter[i].X - mRadius[i], mCircleCenter[i].Y - mRadius[i], mRadius[i] * 2, mRadius[i] * 2);
                    //带个三角箭头
                    Vector2 a = mCenterPath[i + 1];//顶点
                    Vector2 o = mCenterPath[i]; //圆心
                    Vector2 ao = a - o;
                    float r = ao.Length();
                    float d = MathF.Min(20, r/5);//箭头长度
                    float w = MathF.Min(MathF.Max(r / 20, 1), 2);
                    Vector2 v = Vector2.Normalize(ao);
                    Vector2 n = new Vector2(v.Y, -v.X);//法线
                    Vector2 h = a - Vector2.Multiply(v, d);//中垂线点
                    Vector2 b = h + Vector2.Multiply(n, d/2);
                    Vector2 c = h - Vector2.Multiply(n, d / 2);
                    e.Graphics.FillPolygon(Brushes.White, new [] { new PointF(a), new PointF(b), new PointF(c) });
                    int red = (int)(MathF.Min(1, r / 20) * 255);
                    Pen p = new Pen(Color.FromArgb(red,red,red), w);
                    e.Graphics.DrawLine(p, mCircleCenter[i], mCircleCenter[i + 1]);


                }
                if (mFFTPath.Count > 1)
                {
                    e.Graphics.DrawLines(Pens.Yellow, mFFTPath.ToArray());
                }
            }
            if (step == 7)
            {
                e.Graphics.DrawLines(Pens.Yellow, mFFTPath.ToArray());
            }
        }

    }
}
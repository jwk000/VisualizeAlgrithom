using System.Runtime.CompilerServices;

namespace PerlinNoise2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            this.ClientSize = new Size(N, N);
            timer1.Interval = 10;
            timer1.Tick += OnTick;
            timer1.Start();

        }
        const int N = 512;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bm[0] = new Bitmap(N, N);
            bm[1] = new Bitmap(N, N);
            MakeImage(bm[0]);
            MakeImage(bm[1]);
        }


        int ticknum = 30;
        int idx = 0;
        int from = 0, to = 1;
        Bitmap[] bm = new Bitmap[2];
        Bitmap result = new Bitmap(N, N);
        private void OnTick(object? sender, EventArgs e)
        {
            ticknum++;
            if (ticknum == 31)
            {
                ticknum = 1;
                from = idx;
                idx=(idx+1)%2;
                to = idx;
                MakeImage(bm[to]);
            }

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    result.SetPixel(x, y, LerpColor(bm[from].GetPixel(x, y), bm[to].GetPixel(x, y), ticknum / 30f));
                }
            }

            Invalidate();
        }

        Color LerpColor(Color a, Color b, float t)
        {
            int R = (int)(a.R * (1 - t) + b.R * t);
            int G = (int)(a.G * (1 - t) + b.G * t);
            int B = (int)(a.B * (1 - t) + b.B * t);
            return Color.FromArgb(R,G,B);

        }

        void MakeImage(Bitmap bm)
        {
            Perlin.shuffle();//随机数决定了图片整体的明暗
            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    float fx = x*0.02f;
                    float fy = y*0.02f;
                    float n = Perlin.Noise(fx, fy, 0);
                    int r = (int)((0.5f * n + 0.5f) * 255);
                    Color c = Color.FromArgb(r, r, r);
                    bm.SetPixel(x, y, c);
                }
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (result != null)
            {
                e.Graphics.DrawImage(result, 0, 0);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.S && e.Control)
            {
                result.Save("noise.png");
            }
        }
    }

    //https://blog.csdn.net/qq_38680728/article/details/122110828
    class Perlin
    {
        //衰减函数（缓动函数） 6t^5-15t^4+10t^3
        static float fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static float lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        public static void shuffle()
        {
            Random rand = new Random();

            for (int i = 0; i < perm.Length; i++)
            {
                int j = rand.Next(0, perm.Length);
                (perm[i], perm[j]) = (perm[j], perm[i]);
            }
           
        }

        //生成梯度
        public static float Noise(float x, float y, float z)
        {
            // 对应伪代码的第一步，向下取整
            // 这里&0xff作用是让 X 值范围为0-255，注意我们的目的是求Hash，这个操作只是减小随机范围
            var X = (int)MathF.Floor(x) & 0xff;
            var Y = (int)MathF.Floor(y) & 0xff;
            var Z = (int)MathF.Floor(z) & 0xff;

            // 现在 x,y,z已经变成了原来的小数部分
            x -= (int)MathF.Floor(x);
            y -= (int)MathF.Floor(y);
            z -= (int)MathF.Floor(z);

            // 随xyz缓动变化
            var u = fade(x);
            var v = fade(y);
            var w = fade(z);

            //由于输入是连续变化的，其整数部分每个grid变化一次
            //在三维中，我们临近整数点有8个，每个点算一个固定的随机数作为hash值
            int A = (perm[X] + Y) & 0xff;
            int B = (perm[X + 1] + Y) & 0xff;
            int AA = (perm[A] + Z) & 0xff;
            int BA = (perm[B] + Z) & 0xff;
            int AB = (perm[A + 1] + Z) & 0xff;
            int BB = (perm[B + 1] + Z) & 0xff;
            int AAA = perm[AA];
            int BAA = perm[BA];
            int ABA = perm[AB];
            int BBA = perm[BB];
            int AAB = perm[AA + 1];
            int BAB = perm[BA + 1];
            int ABB = perm[AB + 1];
            int BBB = perm[BB + 1];

            float x1, x2, y1, y2;
            // 依次在x,y,z方向插值
            // 次序其实没有关系，但是需要对应，比如下面这句（x,y,z）和（x-1,y,z）说明是在x方向插值
            x1 = lerp(Grad(AAA, x, y, z), Grad(BAA, x - 1, y, z), u);
            x2 = lerp(Grad(ABA, x, y - 1, z), Grad(BBA, x - 1, y - 1, z), u);
            y1 = lerp(x1, x2, v);

            x1 = lerp(Grad(AAB, x, y, z - 1), Grad(BAB, x - 1, y, z - 1), u);
            x2 = lerp(Grad(ABB, x, y - 1, z - 1), Grad(BBB, x - 1, y - 1, z - 1), u);
            y2 = lerp(x1, x2, v);
            return lerp(y1, y2, w);
        }

        // 输入的第一个数值Hash和一个坐标（x,y,z），其作用与 随机梯度和方向向量的点乘 相同
        // 优点在于：
        //     1、方向统一为面上某个角的角平分线方向而不指向立方体内或者坐标值方向，块不会与坐标轴平行
        //	   2、计算更快
        public static float Grad(int hash, float x, float y, float z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y; //对角方向
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }


        private static readonly int[] perm = {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,151
        };
    }

}
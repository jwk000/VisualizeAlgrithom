using System.Runtime.CompilerServices;

namespace PerlinNoise2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            timer1.Interval = 10;
            timer1.Tick += OnTick;
            timer1.Start();

            bm[0] = MakeImage();
            bm[1] = MakeImage();
        }

        int ticknum = 0;
        Bitmap[] bm = new Bitmap[2];
        Bitmap result;
        private void OnTick(object? sender, EventArgs e)
        {
            ticknum++;
            if (ticknum == 30)
            {
                ticknum = 0;
                bm[0] = bm[1];
                bm[1] = MakeImage();
                return;
            }
            var bm1 = bm[0];
            var bm2 = bm[1];
            result = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int x = 0; x < ClientSize.Width; x++)
            {
                for (int y = 0; y < ClientSize.Height; y++)
                {
                    result.SetPixel(x, y, LerpColor(bm1.GetPixel(x, y), bm2.GetPixel(x, y), ticknum / 30f));
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

        Bitmap MakeImage()
        {
            Perlin.shuffle();
            var bm = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int x = 0; x < bm.Width; x++)
            {
                for (int y = 0; y < bm.Height; y++)
                {
                    float fx = (x) / 100f;
                    float fy = (y) / 100f;
                    float n = Perlin.Noise(fx, fy, 0);
                    int r = (int)((0.5f * n + 0.5f) * 255);
                    //Color c = ColorHelper.HSVtoRGB(r, 1, 1);
                    Color c = Color.FromArgb(r, r, r);
                    bm.SetPixel(x, y, c);
                }
            }
            return bm;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (result != null)
            {
                e.Graphics.DrawImage(result, 0, 0);
            }
        }
    }

    class Perlin
    {
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

            // 对应伪代码 smooth，让显示更平滑
            var u = fade(x);
            var v = fade(y);
            var w = fade(z);

            var A = (perm[X] + Y) & 0xff;
            var B = (perm[X + 1] + Y) & 0xff;
            var AA = (perm[A] + Z) & 0xff;
            var BA = (perm[B] + Z) & 0xff;
            var AB = (perm[A + 1] + Z) & 0xff;
            var BB = (perm[B + 1] + Z) & 0xff;

            // 这些都是取Hash操作，在三维中，我们临近整数点有8个，现在得到的类似8个伪随机向量
            // 这里AAA实际上不是向量而是一个伪随机数值，但是后面的 Grad 操作其实会做与伪随机向量类似的事情
            // 原因参见 https://mrl.cs.nyu.edu/~perlin/paper445.pdf
            var AAA = perm[AA];
            var BAA = perm[BA];
            var ABA = perm[AB];
            var BBA = perm[BB];
            var AAB = perm[AA + 1];
            var BAB = perm[BA + 1];
            var ABB = perm[AB + 1];
            var BBB = perm[BB + 1];

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
        // 具体原因参见 https://mrl.cs.nyu.edu/~perlin/paper445.pdf
        public static float Grad(int hash, float x, float y, float z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y;
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
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
            151
        };
    }


    /// <summary>
    /// 颜色转换帮助类
    /// </summary>
    public class ColorHelper
    {
        /// <summary>
        ///  RGB转HSV
        /// </summary>
        /// <param name="red">红色值</param>
        /// <param name="green">绿色值</param>
        /// <param name="blue">蓝色值</param>
        /// <returns>返回：HSV值集合</returns>
        public static List<float> RGBtoHSV(int red, int green, int blue)
        {
            List<float> hsbList = new List<float>();
            System.Drawing.Color dColor = System.Drawing.Color.FromArgb(red, green, blue);
            hsbList.Add(dColor.GetHue());
            hsbList.Add(dColor.GetSaturation());
            hsbList.Add(dColor.GetBrightness());
            return hsbList;
        }


        /// <summary>
        /// HSV转RGB
        /// </summary>
        /// <param name="hue">色调</param>
        /// <param name="saturation">饱和度</param>
        /// <param name="brightness">亮度</param>
        /// <returns>返回：Color</returns>
        public static Color HSVtoRGB(float hue, float saturation, float brightness)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = hue / 60f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromArgb(Convert.ToByte(255), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
    }


}
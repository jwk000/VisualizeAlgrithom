using System;
using System.ComponentModel;

namespace SDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        const int WIDTH = 490;
        const int HEIGHT = 490;

        struct sdPoint
        {
            public int dx, dy;
            public int DistSq() { return dx * dx + dy * dy; }
            public int Dist() { return (int)Math.Sqrt(DistSq()); }
        }
        sdPoint inside = new sdPoint() { dx = 0, dy = 0 };
        sdPoint empty = new sdPoint() { dx = 1000, dy = 1000 };
        sdPoint[,] grid1 = new sdPoint[HEIGHT, WIDTH];
        sdPoint[,] grid2 = new sdPoint[HEIGHT, WIDTH];
        Bitmap sdf = new Bitmap(WIDTH, HEIGHT);

        void Init()
        {

            Bitmap a = new Bitmap("./a.jpg");
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    Color px = a.GetPixel(x, y);
                    if (px.R == 255)
                    {
                        grid1[x,y] = inside;
                        grid2[x,y] = empty;
                    }
                    else
                    {
                        grid2[x,y] = inside;
                        grid1[x,y] = empty;
                    }
                }
            }

            // Generate the SDF.
            GenerateSDF(grid1);
            GenerateSDF(grid2);

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    int red = grid1[x,y].Dist() + grid2[x,y].Dist();
                    Color c = Color.FromArgb(red, red, red);
                    sdf.SetPixel(x, y, c);
                }
            }
            
            Invalidate();
        }

        sdPoint Get(sdPoint[,] g, int x, int y)
        {
            if (x < 0 || y < 0 || x >= WIDTH || y >= HEIGHT)
                return empty;
            return g[x, y];
        }


        sdPoint Compare(sdPoint[,] g, sdPoint p, int x, int y, int offsetx, int offsety)
        {
            sdPoint other = Get(g,x + offsetx, y + offsety);
            other.dx += offsetx;
            other.dy += offsety;

            if (other.DistSq() < p.DistSq())
                p = other;

            return p;
        }

        void GenerateSDF(sdPoint[,] g)
        {
            // Pass 0
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    sdPoint p = Get(g,x, y);
                    p = Compare(g, p, x, y, -1, 0);
                    p = Compare(g, p, x, y, 0, -1);
                    p = Compare(g, p, x, y, -1, -1);
                    p = Compare(g, p, x, y, 1, -1);
                    g[x, y] = p;
                }

                for (int x = WIDTH - 1; x >= 0; x--)
                {
                    sdPoint p = Get(g, x, y);
                    p = Compare(g, p, x, y, 1, 0);
                    g[x, y] = p;
                }
            }

            // Pass 1
            for (int y = HEIGHT - 1; y >= 0; y--)
            {
                for (int x = WIDTH - 1; x >= 0; x--)
                {
                    sdPoint p = Get(g,x, y);
                    p = Compare(g, p, x, y, 1, 0);
                    p = Compare(g, p, x, y, 0, 1);
                    p = Compare(g, p, x, y, -1, 1);
                    p = Compare(g, p, x, y, 1, 1);
                    g[x, y] = p;
                }

                for (int x = 0; x < WIDTH; x++)
                {
                    sdPoint p = Get(g, x, y);
                    p = Compare(g, p, x, y, -1, 0);
                    g[x, y] = p;
                }
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawImage(sdf, 0, 0);

        }
    }





}

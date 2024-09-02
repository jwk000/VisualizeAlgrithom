using System;
using System.ComponentModel;

namespace SDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        const int WIDTH = 256;
        const int HEIGHT = 256;

        struct sdPoint
        {
            public int dx, dy;
            public int DistSq() { return dx * dx + dy * dy; }
        }

        class sdGrid
        {
            public sdPoint[,] grid = new sdPoint[WIDTH, HEIGHT];
        }


        sdPoint Get(sdGrid g, int x, int y)
        {
            // OPTIMIZATION: you can skip the edge check code if you make your grid 
            // have a 1-pixel gutter.
            if (x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT)
                return g.grid[y, x];
            else
                return default;
        }

        void Put(sdGrid g, int x, int y, sdPoint p)
        {
            g.grid[y, x] = p;
        }

        void Compare(sdGrid g, sdPoint p, int x, int y, int offsetx, int offsety)
        {
            sdPoint other = Get(g, x + offsetx, y + offsety);
            other.dx += offsetx;
            other.dy += offsety;

            if (other.DistSq() < p.DistSq())
                p = other;
        }

        void GenerateSDF(sdGrid g)
        {
            // Pass 0
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    sdPoint p = Get(g, x, y);
                    Compare(g, p, x, y, -1, 0);
                    Compare(g, p, x, y, 0, -1);
                    Compare(g, p, x, y, -1, -1);
                    Compare(g, p, x, y, 1, -1);
                    Put(g, x, y, p);
                }

                for (int x = WIDTH - 1; x >= 0; x--)
                {
                    sdPoint p = Get(g, x, y);
                    Compare(g, p, x, y, 1, 0);
                    Put(g, x, y, p);
                }
            }

            // Pass 1
            for (int y = HEIGHT - 1; y >= 0; y--)
            {
                for (int x = WIDTH - 1; x >= 0; x--)
                {
                    sdPoint p = Get(g, x, y);
                    Compare(g, p, x, y, 1, 0);
                    Compare(g, p, x, y, 0, 1);
                    Compare(g, p, x, y, -1, 1);
                    Compare(g, p, x, y, 1, 1);
                    Put(g, x, y, p);
                }

                for (int x = 0; x < WIDTH; x++)
                {
                    sdPoint p = Get(g, x, y);
                    Compare(g, p, x, y, -1, 0);
                    Put(g, x, y, p);
                }
            }
        }

        void main()
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    Uint8 r, g, b;
                    Uint32* src = ((Uint32*)((Uint8*)temp->pixels + y * temp->pitch)) + x;
                    SDL_GetRGB(*src, temp->format, &r, &g, &b);
                    // Points inside get marked with a dx/dy of zero.
                    // Points outside get marked with an infinitely large distance.
                    if (g < 128)
                    {
                        Put(grid1, x, y, inside);
                        Put(grid2, x, y, empty);
                    }
                    else
                    {
                        Put(grid2, x, y, inside);
                        Put(grid1, x, y, empty);
                    }
                }
            }

            // Generate the SDF.
            GenerateSDF(grid1);
            GenerateSDF(grid2);

        }
    }





}

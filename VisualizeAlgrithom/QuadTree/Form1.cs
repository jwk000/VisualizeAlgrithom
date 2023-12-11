using System;

namespace QuadTree
{
    //ËÄ²æÊ÷ÑÝÊ¾Ëã·¨
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.timer1.Tick += OnTick;
            this.timer1.Interval = 30;
            this.timer1.Start();
        }

        const int W = 800;
        const int H = 800;
        const int ObjMinSize = 10;
        const int ObjMaxSize = 30;
        Random randor = new Random();
        QuadTree qdtree = new QuadTree(5, 4, 0, W, 0, H);
        void Init()
        {
            this.ClientSize = new Size(W, H);

            for (int i = 0; i < 80; i++)
            {
                int x = randor.Next(W - ObjMaxSize);
                int y = randor.Next(H - ObjMaxSize);
                int w = randor.Next(ObjMinSize, ObjMaxSize);
                int h = randor.Next(ObjMinSize, ObjMaxSize);
                QuadTree.QTObject obj = new QuadTree.QTObject(x, y, w, h);

                qdtree.AddObject(obj);
            }

            Invalidate();

        }
        private void OnTick(object? sender, EventArgs e)
        {
            onTickQuadTree(qdtree, qdtree.AllObjects[1]);
        }

        void onTickQuadTree(QuadTree tree, QuadTree.QTObject obj)
        {
            int x = randor.Next(0, 30);
            int y = randor.Next(0, 10);
            obj.X += x;
            obj.Y += y;
            if (obj.X + obj.W < 0)
            {
                obj.X = 800;
            }
            if (obj.X > 800)
            {
                obj.X = 0;
            }
            if (obj.Y + obj.H < 0)
            {
                obj.Y = 800;
            }
            if (obj.Y > 800)
            {
                obj.Y = 0;
            }
            tree.UpdateObject(obj);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            drawQuadTree(e.Graphics, qdtree);
        }

        void drawQuadTree(Graphics g, QuadTree tree)
        {
            foreach (var obj in tree.AllObjects.Values)
            {
                g.DrawRectangle(Pens.Black, obj.X, obj.Y, obj.W, obj.H);
            }
            var me = tree.AllObjects[1];
            g.FillRectangle(Brushes.Green, me.X, me.Y, me.W, me.H);
            drawQTNode(g, tree.Root);
            foreach (var o in tree.GetIntreastObjects(me))
            {
                g.FillRectangle(Brushes.Red, o.X, o.Y, o.W, o.H);
            }
        }

        Pen[] QuadTreePens = new Pen[5]
        {
            Pens.DarkGreen,Pens.DodgerBlue,Pens.Orange,Pens.Brown,Pens.Cyan
        };
        void drawQTNode(Graphics g, QuadTree.QTNode node)
        {
            int width = node.Right - node.Left;
            int height = node.Down - node.Up;
            Pen p = QuadTreePens[node.level];
            g.DrawRectangle(p, node.Left, node.Up, width - 1, height - 1);
            if (node.SubNodes != null)
            {
                foreach (var n in node.SubNodes)
                {
                    drawQTNode(g, n);
                }
            }
            else
            {
                g.DrawString(node.Objects.Count.ToString(), SystemFonts.DefaultFont, Brushes.Red, node.Left + width / 2, node.Up + height / 2);

            }

        }

    }
}
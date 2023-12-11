using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initGrids();
            initTimer();
            initAnts();
            this.Width = m_kXCount * m_kGridSize + m_kGridSize;
            this.Height = m_kYCount * m_kGridSize + m_kGridSize;
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
        }
        //网格对象
        class Grid
        {
            public Rectangle rect;
            public bool black;
            public Point pos;//topleft
        }

        const int m_kGridSize = 10;
        const int m_kXCount = 180;
        const int m_kYCount = 90;
        const int m_kAntCount = 120;

        Grid[,] m_allGrids = new Grid[m_kXCount, m_kYCount];
        void initGrids()
        {
            for(int x=0;x<m_kXCount;x++)
            {
                for(int y=0;y<m_kYCount;y++)
                {
                    m_allGrids[x, y] = new Grid
                    {
                        black = false,
                        pos = new Point(x * m_kGridSize, y * m_kGridSize),
                        rect = new Rectangle(x * m_kGridSize, y * m_kGridSize, m_kGridSize, m_kGridSize),
                    };
                }
            }
        }

        class TheAnt
        {
            public Point pos = new Point(m_kXCount / 2, m_kYCount / 2);
            public int direction;//朝向 0上1右2下3左
            public Rectangle rect = new Rectangle(m_kXCount / 2*m_kGridSize+m_kGridSize/4, m_kYCount / 2*m_kGridSize+m_kGridSize/4, m_kGridSize / 2, m_kGridSize / 2);
        }

        List<TheAnt> m_ants = new List<TheAnt>();
        Random rander = new Random();
        void initAnts()
        {
            for(int i=0;i< m_kAntCount;i++)
            {
                int x = rander.Next(10, m_kXCount - 10);
                int y = rander.Next(10, m_kYCount - 10);
                TheAnt ant = new TheAnt();
                ant.pos.X = x;
                ant.pos.Y = y;
                ant.direction = rander.Next(0, 3);
                ant.rect.X = x * m_kGridSize + m_kGridSize / 4;
                ant.rect.Y = y * m_kGridSize + m_kGridSize / 4;
                m_ants.Add(ant);

            }
        }
        Size[] m_Roffset = new Size[4]
        {
            new Size( 1,0),
            new Size(0,1),
            new Size(-1,0),
            new Size(0,-1)
        };

        Size[] m_Loffset = new Size[4] {
            new Size(-1,0),
            new Size(0,-1),
            new Size(1,0),
            new Size(0,1)
        };

        void moveNext(TheAnt ant)
        {
            int r = rander.Next(0, 100);
            if(m_allGrids[ant.pos.X, ant.pos.Y].black)
            //if(r<60)
            {
                //黑-右转
                ant.pos += m_Roffset[ant.direction];
                ant.direction = (ant.direction + 1) % 4;
            }
            else
            {
                //白-左转
                ant.pos += m_Loffset[ant.direction];
                ant.direction = (ant.direction + 1) % 4;
            }
            //越界穿透
            if (ant.pos.X < 0) ant.pos.X = m_kXCount - 1;
            if (ant.pos.Y < 0) ant.pos.Y = m_kYCount - 1;
            if (ant.pos.X == m_kXCount) ant.pos.X = 0;
            if (ant.pos.Y == m_kYCount) ant.pos.Y = 0; 
            ant.rect.X = m_allGrids[ant.pos.X, ant.pos.Y].rect.Left + m_kGridSize / 4;
            ant.rect.Y = m_allGrids[ant.pos.X, ant.pos.Y].rect.Top + m_kGridSize / 4;
            m_allGrids[ant.pos.X, ant.pos.Y].black = !m_allGrids[ant.pos.X, ant.pos.Y].black;
        }
        //定时器
        Timer m_timer = new Timer();
        void initTimer()
        {
            m_timer.Interval = 10;//蚂蚁移动速度
            m_timer.Tick += onTimerTick;
            m_timer.Enabled = false;
        }

        private void onTimerTick(object sender, EventArgs e)
        {
            //判断当前状态，计算下一个位置
            foreach (var ant in m_ants)
            {
                moveNext(ant);
            }
            Invalidate();
        }

        //键盘控制启动暂停
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.Space)
            {
                m_timer.Enabled = !m_timer.Enabled;
            }
        }

        Pen m_gridPen = new Pen(Color.LightGray, 1);
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            //绘制网格
            for(int x=0;x<m_kXCount;x++)
            {
                for(int y=0;y<m_kYCount;y++)
                {
                    var grid = m_allGrids[x, y];
                    if (grid.black)
                    {
                        g.FillRectangle(Brushes.Brown, grid.rect);
                    }
                    g.DrawRectangle(m_gridPen, grid.rect);
                }
            }
            //绘制蚂蚁
            foreach (var ant in m_ants)
            {
                g.FillEllipse(Brushes.DarkSeaGreen, ant.rect);
            }
        }


    }
}

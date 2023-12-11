using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cell
{
    //康威生命游戏
    //每个细胞有两种状态 - 存活或死亡，每个细胞与以自身为中心的周围八格细胞产生互动。（如图，黑色为存活，白色为死亡）
    //当前细胞为存活状态时，当周围低于2个（不包含2个）存活细胞时， 该细胞变成死亡状态。（模拟生命数量稀少）
    //当前细胞为存活状态时，当周围有2个或3个存活细胞时， 该细胞保持原样。
    //当前细胞为存活状态时，当周围有3个以上的存活细胞时，该细胞变成死亡状态。（模拟生命数量过多）
    //当前细胞为死亡状态时，当周围有3个存活细胞时，该细胞变成存活状态。 （模拟繁殖）

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = m_kXCount * m_kGridSize + m_kGridSize;
            this.Height = m_kYCount * m_kGridSize + m_kGridSize;
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            initGrids();
            initTimer();
        }

        //网格对象
        class Grid
        {
            public Rectangle rect;
            public bool alive;
            public bool next;
            public Point pos;//topleft
        }

        const int m_kGridSize = 20;
        const int m_kXCount = 100;
        const int m_kYCount = 60;
        const int m_kAntCount = 1;

        Grid[,] m_allGrids = new Grid[m_kXCount, m_kYCount];
        void initGrids()
        {
            for (int x = 0; x < m_kXCount; x++)
            {
                for (int y = 0; y < m_kYCount; y++)
                {
                    m_allGrids[x, y] = new Grid
                    {
                        alive = false,
                        pos = new Point(x, y),
                        rect = new Rectangle(x * m_kGridSize, y * m_kGridSize, m_kGridSize, m_kGridSize),
                    };
                }
            }
        }

        //细胞
        List<Grid> m_allCells = new List<Grid>();

        //定时器
        Timer m_timer = new Timer();
        void initTimer()
        {
            m_timer.Interval = 100;//蚂蚁移动速度
            m_timer.Tick += onTimerTick;
            m_timer.Enabled = false;
        }

        Size[] m_8offset = new Size[8]
        {
            new Size(-1,-1),
            new Size(0,-1),
            new Size(1,-1),
            new Size(-1,0),
            new Size(1,0),
            new Size(-1,1),
            new Size(0,1),
            new Size(1,1)
        };

        private void onTimerTick(object sender, EventArgs e)
        {

            List<Grid> newCells = new List<Grid>();
            List<Grid> checkedCell = new List<Grid>();
            foreach(var cell in m_allCells)
            {
                int cnt = 0;
                foreach(var s in m_8offset)
                {
                    var p = cell.pos + s;
                    if (p.X < 0) p.X = m_kXCount - 1;
                    if (p.Y < 0) p.Y = m_kYCount - 1;
                    if (p.X == m_kXCount) p.X = 0;
                    if (p.Y == m_kYCount) p.Y = 0;

                    var _cell = m_allGrids[p.X, p.Y];
                    if (_cell.alive)
                    {
                        cnt++;
                    }
                    else
                    {
                        if (checkedCell.Contains(_cell))
                            continue;

                        checkedCell.Add(_cell);

                        //死亡状态的细胞
                        int _cnt = 0;
                        foreach(var _s in m_8offset)
                        {
                            var _p = _cell.pos + _s;
                            if (_p.X < 0) _p.X = m_kXCount - 1;
                            if (_p.Y < 0) _p.Y = m_kYCount - 1;
                            if (_p.X == m_kXCount) _p.X = 0;
                            if (_p.Y == m_kYCount) _p.Y = 0;

                            var __cell = m_allGrids[_p.X, _p.Y];
                            if (__cell.alive)
                            {
                                _cnt++;
                            }
                        }
                        if (_cnt == 3)
                        {
                            _cell.next = true;
                            newCells.Add(_cell);
                        }
                    }
                }
                if (cnt < 2 || cnt > 3)
                    cell.next = false;
                else
                    cell.next = true;
            }
            foreach(var cell in m_allCells)
            {
                cell.alive = cell.next;
            }
            foreach(var cell in newCells)
            {
                cell.alive = cell.next;
            }
            m_allCells.RemoveAll(c => !c.alive);
            m_allCells.AddRange(newCells);
            Invalidate();
        }

        //鼠标选择活细胞
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (m_isRunning) return;
            //计算位置
            int x = e.X / m_kGridSize;
            int y = e.Y / m_kGridSize;
            var grid = m_allGrids[x, y];
            grid.alive = true;
            m_allCells.Add(grid);

            Invalidate();
        }

        bool m_isRunning = false;
        //键盘控制启动暂停
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                m_isRunning = !m_isRunning;
                m_timer.Enabled = !m_timer.Enabled;
            }
        }

        Pen m_gridPen = new Pen(Color.Black, 1);
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            //绘制网格
            for (int x = 0; x < m_kXCount; x++)
            {
                for (int y = 0; y < m_kYCount; y++)
                {
                    var grid = m_allGrids[x, y];
                    if (grid.alive)
                    {
                        g.FillRectangle(Brushes.Brown, grid.rect);
                    }
                    g.DrawRectangle(m_gridPen, grid.rect);
                }
            }
        }

    }
}

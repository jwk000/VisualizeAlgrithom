using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberRain
{
    public partial class Form1 : Form
    {
        //随机数
        Random mRandGen = new Random();
        //场景大小
        int mSceneLeftX = 0;
        int mSceneRightX = 0;
        int mSceneUpY = 0;
        int mSceneDownY = 0;
        //边框
        int mSceneMargen = 10;
        //最大掉落速度
        const int mRunSpeed = 10;
        //雨滴生成周期
        const int mRainDropCircle = 10;
        //数字变换速度
        const int mNumberSpeed = 10;
        //字体大小
        const int mFontSize = 14;


        //雨滴
        class RainDrop
        {
            public Point _point;
            public int _length;
            public int _speed;
            public int _lightIndex;
            public int _numCircle;
            public int _lightCircle;
            public string[] _body;
        }
        //全部雨
        List<RainDrop> mAllRains = new List<RainDrop>();
        //字母表
        string[] mNumberTable = new string[]
        {
            "1","2","3","4","5","6","7","8","9","0",
            "-","=","_","+","`","~","!","@","#","$",
            "%","^","&","*","(",")","[","]","{","}",
            ".","|",";",":","'","?",",","<",">","/",
            "a","b","c","d","e","f","g","h","i","j",
            "k","l","m","n","o","p","q","r","s","t",
            "u","v","w","x","y","z","A","B","C","D",
            "E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X",
            "Y","Z",

        };

        public Form1()
        {
            InitializeComponent();
            InitForm();
        }

        //初始化窗口
        void InitForm()
        {
            //窗口全屏
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = Screen.PrimaryScreen.Bounds.Size;

            mSceneLeftX = mSceneMargen;
            mSceneUpY = mSceneMargen;
            mSceneRightX = ClientSize.Width - mSceneMargen;
            mSceneDownY = ClientSize.Height - mSceneMargen;

            //窗口背景
            this.BackColor = Color.Black;
            //隐藏鼠标
            Cursor.Hide();
            this.ShowInTaskbar = false;
            //双帧缓冲打开
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //定时器
            UITimer.Enabled = true;
            UITimer.Interval = mRunSpeed;
            UITimer.Tick += UITimer_Tick;
            UITimer.Start();
        }

        RainDrop GenerateRainDrop()
        {
            int x = mRandGen.Next(mSceneLeftX, mSceneRightX);
            int y = mSceneUpY;

            RainDrop drop = new RainDrop()
            {
                _point = new Point(x, y),
                _length = mRandGen.Next(5, 20),
                _speed = mRandGen.Next(3, mNumberSpeed),
            };

            drop._lightIndex = drop._length - 1;
            drop._body = new string[drop._length];
            for(int i=0;i<drop._length;i++)
            {
                drop._body[i] = mNumberTable[mRandGen.Next(0, mNumberTable.Length)];
            }

            return drop;
        }

        void AddRainDrops(int num)
        {
            for (int i = 0; i < num; i++)
            {
                mAllRains.Add(GenerateRainDrop());
            }
        }


        int mCurStep = 0;
        private void UITimer_Tick(object sender, EventArgs e)
        {
            //生成雨滴
            if(++mCurStep == mRainDropCircle)
            {
                mCurStep = 0;
                AddRainDrops(10);
            }

            List<RainDrop> toDel = new List<RainDrop>();
            //雨滴掉落
            foreach(var drop in mAllRains)
            {
                if(drop._point.Y - drop._length* mFontSize > mSceneDownY - mSceneUpY)
                {
                    toDel.Add(drop);
                    continue;
                }
                drop._point.Y += drop._speed;//下落

                if(++drop._lightCircle == 2*(mNumberSpeed-drop._speed))
                {
                    drop._lightCircle = 0;
                    //高亮字母位置
                    if (drop._lightIndex > 0)
                    {
                        drop._lightIndex--;
                    }
                }

                if (++drop._numCircle == mNumberSpeed - drop._speed)
                {
                    drop._numCircle = 0;

                    //重新计算字母
                    for (int i = 0; i < drop._length; i++)
                    {
                        drop._body[i] = mNumberTable[mRandGen.Next(0, mNumberTable.Length)];
                    }
                }

            }
            //雨滴销毁
            foreach (var drop in toDel)
            {
                mAllRains.Remove(drop);
            }

            //重绘
            this.Invalidate(new Rectangle(0, 0, this.Size.Width, this.Size.Height));

        }

        Font _dropFont = new Font("Arail", mFontSize);
        Brush _rainBrush = new SolidBrush(Color.Green);
        Brush _lightBrush = new SolidBrush(Color.White);
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            //绘制雨滴
            foreach(RainDrop drop in mAllRains)
            {
                for (int i = 0; i < drop._length; i++)
                {
                    Brush _b = _rainBrush;
                    if(i  == drop._lightIndex)
                    {
                        _b = _lightBrush;
                    }
                    //计算每个字母的位置
                    Point _p = new Point(drop._point.X, drop._point.Y - (mFontSize+5) * i);
                    g.DrawString(drop._body[i], _dropFont, _b, _p);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        enum DoorOfRoom
        {
            UpDoor,RightDoor,DownDoor,LeftDoor
        }

        class Room
        {
            public int _id;
            public bool _visited = false;
            public int _X;
            public int _Y;
            public int _drawX;
            public int _drawY;
        }

        //迷宫位置
        const int kMargen = 50;
        //迷宫大小
        const int kRoomSize = 20;
        int kRoomNumX = 0;
        int kRoomNumY = 0;
        //所有房间
        Room[,] AllRooms = null;
        //随机数
        Random RandGen = new Random();
        //没访问过的房间
        List<Room> NotVisitedRooms = new List<Room>();
        //并发访问数
        const int kThreadNum = 160;

        public Form1()
        {
            InitializeComponent();
            InitForm();
            InitRoom();
            InitRoomLine();
            InitTimer();
            Restart();
        }

        void InitForm()
        {
            //窗口样式
            this.FormBorderStyle = FormBorderStyle.None;
            //颜色
            this.BackColor = Color.Black;
            //位置
            this.StartPosition = FormStartPosition.CenterScreen;
            //大小
            this.ClientSize = Screen.PrimaryScreen.Bounds.Size;
            //双帧缓冲
            this.DoubleBuffered = true;
            //鼠标隐藏
            Cursor.Hide();

        }

        void InitTimer()
        {
            this.UITimer.Enabled = true;
            this.UITimer.Interval = 100;
            this.UITimer.Tick += OnTimerTick;
        }

        //初始化房间
        void InitRoom()
        {
            //计算房间数
            kRoomNumX = (ClientSize.Width - kMargen * 2) / kRoomSize;
            kRoomNumY = (ClientSize.Height - kMargen * 2) / kRoomSize;
            AllRooms = new Room[kRoomNumX, kRoomNumY];

            for (int i=0;i<kRoomNumX;i++)
            {
                for(int j=0;j<kRoomNumY;j++)
                {
                    AllRooms[i, j] = new Room()
                    {
                        _X = i,
                        _Y = j,
                        _drawX = kMargen + i * kRoomSize,
                        _drawY = kMargen + j * kRoomSize,
                        _visited = false
                    };
                }
            }
        }

        void ResetRooms()
        {
            foreach (Room r in AllRooms)
            {
                r._id = 0;
                r._visited = false;
                NotVisitedRooms.Add(r);
            }

        }

        Room GetRoomOnPos(int x, int y)
        {
            if (x < 0 || x >= kRoomNumX) return null;
            if (y < 0 || y >= kRoomNumY) return null;
            return AllRooms[x, y];
        }

        Room GetRoomByDoor(Room room, int doorid)
        {
            if (room == null) return null;
            if (doorid < 0 || doorid > 3) return null;
            switch ((DoorOfRoom)doorid)
            {
                case DoorOfRoom.UpDoor:
                    return GetRoomOnPos(room._X, room._Y - 1);
                case DoorOfRoom.RightDoor:
                    return GetRoomOnPos(room._X + 1, room._Y);
                case DoorOfRoom.DownDoor:
                    return GetRoomOnPos(room._X, room._Y + 1);
                case DoorOfRoom.LeftDoor:
                    return GetRoomOnPos(room._X - 1, room._Y);
            }
            return null;
        }



        #region 算法 DFS
        enum A1Step
        {
            GetNotVisitedRoom,
            VisitNextRoom,
        }

        //先把所有房间加入未访问房间表，取出第一个房间开始随机深度访问其他房间，直到不能继续（周围房间都访问了）
        //然后从未访问房间表中删除上面已经访问的房间，重复上面的过程，直到未访问房间表为空，则全部房间都联通。

        class A1Thread
        {
            public A1Step CurrentStep1 = A1Step.GetNotVisitedRoom;
            public Room CurrentStepRoom = null;
            public HashSet<Room> LastStepRooms = new HashSet<Room>();
        }
        //多线程DFS
        A1Thread[] ths = new A1Thread[kThreadNum];

        void A1Reset()
        {
            for(int i =0;i<ths.Length;i++)
            {
                ths[i] = new A1Thread();
            }
        }


        //同时进行多个dfs
        void A1_Tick()
        {
            foreach(A1Thread t in ths)
            {
                A1_TickOnce(t);
            }
        }

        //每次进行计算的步骤
        void A1_TickOnce(A1Thread th)
        {
            if(th.CurrentStep1 == A1Step.GetNotVisitedRoom)
            {
                if(NotVisitedRooms.Count == 0)
                {
                    //结束生成
                    UITimer.Stop();
                    Restart();
                    return;
                }
                int idx = RandGen.Next(0, NotVisitedRooms.Count);
                idx = 0;
                th.CurrentStepRoom = NotVisitedRooms[idx];
                th.CurrentStepRoom._visited = true;
                NotVisitedRooms.Remove(th.CurrentStepRoom);
                th.LastStepRooms.Clear();
                th.LastStepRooms.Add(th.CurrentStepRoom);
                th.CurrentStep1 = A1Step.VisitNextRoom;
            }

            if(!A1_VisitNextRoom(th))
            {
                th.CurrentStep1 = A1Step.GetNotVisitedRoom;
            }
        }

        bool A1_VisitNextRoom(A1Thread th)
        {
            int doorid = RandGen.Next(0, 4);
            Room room = null;
            bool validRoom = false;
            for(int i = 0;i<4; i++)
            {
                room = GetRoomByDoor(th.CurrentStepRoom, doorid);
                if (null == room || th.LastStepRooms.Contains(room))
                {
                    doorid = (doorid + 1) & 3;
                }else
                {
                    validRoom = true;
                    break;
                }
            }
            if (!validRoom) return false;

            OpenRoomDoor(th.CurrentStepRoom, doorid);

            if(room._visited)
            {
                return false;
            }
            room._visited = true;
            th.CurrentStepRoom = room;
            th.LastStepRooms.Add(th.CurrentStepRoom);
            NotVisitedRooms.Remove(th.CurrentStepRoom);
            return true;
        }
        #endregion

        #region 算法 Kruskal
        //随机取房间，随机一扇门，联通2个房间，所有联通的房间作为一个集合
        //两个集合联通合并为一个集合，如果所有房间都联通但是还有集合，则联通所有集合
        Dictionary<int, List<Room>> A2RoomSets = new Dictionary<int, List<Room>>();
        int AutoIncID = 0;

        void A2Reset()
        {
            AutoIncID = 0;
            A2RoomSets.Clear();
        }
        void A2_Tick()
        {
            if(0 == NotVisitedRooms.Count)
            {
                //只有一个联通集合表示全部联通
                if (A2RoomSets.Count == 1)
                {
                    UITimer.Stop();
                    //重新开始
                    Restart();
                    return;
                }else //合并集合
                {
                    //一次并行多个合并
                    for(int i=0;i<kThreadNum;i++)
                    {
                        if(A2RoomSets.Count>1)
                        {
                            A2MergeOneRoomSet();
                        }
                    }
                }
            }else//访问随机room
            {
                //一次并行多个
                for(int i=0;i< kThreadNum; i++)
                {
                    if (NotVisitedRooms.Count > 0)
                    {
                        A2_VisitNextRoom();
                    }
                }
            }
        }

        void A2_VisitNextRoom()
        {
            //随机room
            int idx = RandGen.Next(0, NotVisitedRooms.Count);
            Room room = NotVisitedRooms[idx];
            NotVisitedRooms.RemoveAt(idx);
            //随机门
            int doorid = RandGen.Next(0, 4);
            Room nextRoom = GetRoomByDoor(room, doorid);
            if(null == nextRoom)
            {
                for (int i = 0; i < 4; i++)
                {
                    doorid = (doorid + 1) & 3;
                    nextRoom = GetRoomByDoor(room, doorid);
                    if(nextRoom!=null)
                    {
                        break;
                    }
                }
            }

            if (nextRoom._visited)
            {
                //附近房间访问过直接加入对方的集合
                room._id = nextRoom._id;
                A2RoomSets[room._id].Add(room);
            }
            else
            {
                //附近的房间没访问过就建立一个集合
                room._id = AutoIncID++;
                nextRoom._id = room._id;
                nextRoom._visited = true;
                List<Room> roomSet = new List<Room>();
                roomSet.Add(room);
                roomSet.Add(nextRoom);
                A2RoomSets.Add(room._id, roomSet);
                //忘了去掉刚访问的room，曾出现了bug，应该封装成一个接口
                NotVisitedRooms.Remove(nextRoom);
            }
            room._visited = true;
            OpenRoomDoor(room, doorid);//唯一与UI相关的借口

        }

        void A2MergeOneRoomSet()
        {
            int firstID = A2RoomSets.Keys.First();
            var OneRoomSet = A2RoomSets[firstID];
            A2RoomSets.Remove(firstID);
            foreach (Room r in OneRoomSet)
            {
                for (int i = 0; i < 4; i++)
                {
                    Room nxt = GetRoomByDoor(r, i);
                    if (nxt == null) continue;

                    if (nxt._id != r._id)
                    {
                        //改变原集合id
                        foreach (Room one in OneRoomSet)
                        {
                            one._id = nxt._id;
                        }
                        A2RoomSets[nxt._id].AddRange(OneRoomSet);
                        //别忘了合并也要开门
                        OpenRoomDoor(r, i);
                        return;
                    }
                }
            }

        }
        #endregion

        #region 反应堆算法 reactor
        //随机选择一个点加入已访问列表，每次选择2个door打开，新加入的room作为新的反应源递归打开2个door
        List<Room> ReactorSrc = new List<Room>();

        void A3Reset()
        {
            ReactorSrc.Clear();
            //随机room
            int idx = RandGen.Next(0, NotVisitedRooms.Count);
            Room room = NotVisitedRooms[idx];
            NotVisitedRooms.RemoveAt(idx);
            room._visited = true;
            ReactorSrc.Add(room);
        }

        void A3_Tick()
        {
            if(NotVisitedRooms.Count == 0)
            {
                UITimer.Stop();
                Restart();
                return;
            }

            List<Room> nextReactor = new List<Room>();
            //随机门
            while(ReactorSrc.Count>0)
            {
                int idx = RandGen.Next(0, ReactorSrc.Count);
                Room room = ReactorSrc[idx];
                ReactorSrc.RemoveAt(idx);
                for(int i=0;i<2;i++)
                {
                    int doorid = 0;
                    Room nextRoom = A3GetRoomByRandDoor(room, out doorid);
                    if (nextRoom == null)
                    {
                        continue;
                    }
                    OpenRoomDoor(room, doorid);
                    nextRoom._visited = true;
                    nextReactor.Add(nextRoom);
                    NotVisitedRooms.Remove(nextRoom);
                }
            }
            ReactorSrc = nextReactor;
        }

        Room A3GetRoomByRandDoor(Room room, out int _doorid)
        {
            int doorid = RandGen.Next(0, 4);
            _doorid = doorid;
            for (int i = 0; i < 4; i++)
            {
                Room nextRoom = GetRoomByDoor(room, doorid);
                if (nextRoom != null && !nextRoom._visited)
                {
                    _doorid = doorid;
                    return nextRoom;
                }
                doorid = (doorid + 1) & 3;
            }

            //返回空说明无门可走了
            return null;
        }
        #endregion

        delegate void TickAlgrithom();
        TickAlgrithom Ticker = null;

        void Restart()
        {
            ResetRooms();
            ResetRoomLine();

            //画笔颜色也改掉
            int r = RandGen.Next(80, 255);
            int g = RandGen.Next(80, 255);
            int b = RandGen.Next(80, 255);
            doorPen = new Pen(Color.FromArgb(r,g,b), 2);
            //随机选择一个算法，然后初始化并开始
            int alg = RandGen.Next(1, 4);
            //alg = 2;
            if (alg == 1)
            {
                A1Reset();
                Ticker = A1_Tick;
            }
            else if (alg == 2)
            {
                A2Reset();
                Ticker = A2_Tick;
            }
            else if (alg == 3)
            {
                A3Reset();
                Ticker = A3_Tick;
            }

            System.Threading.Thread.Sleep(1000);
            UITimer.Start();
        }


        private void OnTimerTick(object sender, EventArgs e)
        {
            //生成迷宫
            Ticker();
            //重绘
            this.Invalidate(new Rectangle(new Point(kMargen, kMargen), this.ClientSize));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //绘制所有房间
            DrawAllRoomDoor(e.Graphics);
        }

        //线段管理器
        class RoomLine
        {
            public Point A, B;
            public bool hide;
        }
        int kLineNum = 0;

        RoomLine[,] RoomLineX = null;
        RoomLine[,] RoomLineY = null;

        void InitRoomLine()
        {
            kLineNum = kRoomNumX * (kRoomNumY + 1) + kRoomNumY * (kRoomNumX + 1);
            RoomLineX = new RoomLine[kRoomNumX, kRoomNumY + 1];
            RoomLineY = new RoomLine[kRoomNumX + 1, kRoomNumY];


            foreach (Room room in AllRooms)
            {
                //up
                RoomLineX[room._X, room._Y] = new RoomLine()
                {
                    A = new Point(room._drawX, room._drawY),
                    B = new Point(room._drawX + kRoomSize, room._drawY)
                };
                //down
                RoomLineX[room._X, room._Y + 1] = new RoomLine()
                {
                    A = new Point(room._drawX, room._drawY + kRoomSize),
                    B = new Point(room._drawX + kRoomSize, room._drawY + kRoomSize)
                };
                //left
                RoomLineY[room._X, room._Y] = new RoomLine()
                {
                    A = new Point(room._drawX, room._drawY),
                    B = new Point(room._drawX, room._drawY + kRoomSize)
                };
                //right
                RoomLineY[room._X + 1, room._Y] = new RoomLine()
                {
                    A = new Point(room._drawX+kRoomSize, room._drawY),
                    B = new Point(room._drawX+kRoomSize, room._drawY+kRoomSize)
                };
            }
        }

        void ResetRoomLine()
        {
            foreach(RoomLine r in RoomLineX)
            {
                r.hide = false;
            }
            foreach(RoomLine r in RoomLineY)
            {
                r.hide = false;
            }
        }
        void OpenRoomDoor(Room room, int doorid)
        {
            switch((DoorOfRoom)doorid)
            {
                case DoorOfRoom.UpDoor:
                    RoomLineX[room._X, room._Y].hide = true;
                    break;
                case DoorOfRoom.DownDoor:
                    RoomLineX[room._X, room._Y + 1].hide = true;
                    break;
                case DoorOfRoom.RightDoor:
                    RoomLineY[room._X + 1, room._Y].hide = true;
                    break;
                case DoorOfRoom.LeftDoor:
                    RoomLineY[room._X, room._Y].hide = true;
                    break;
            }
        }

        Pen doorPen = new Pen(Color.LightGray, 2);
        //绘制全部房间
        void DrawAllRoomDoor(Graphics g)
        {
            foreach(RoomLine line in RoomLineX)
            {
                if(!line.hide)
                {
                    g.DrawLine(doorPen, line.A, line.B);
                }
            }
            foreach (RoomLine line in RoomLineY)
            {
                if (!line.hide)
                {
                    g.DrawLine(doorPen, line.A, line.B);
                }
            }
        }

        //按键
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

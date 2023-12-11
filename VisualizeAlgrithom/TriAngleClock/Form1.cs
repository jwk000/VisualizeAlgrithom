using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriAngle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        Timer timer = new Timer();
        float centerX = 0;
        float centerY = 0;
        float r = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //地盘
            centerX = ClientSize.Width / 2;
            centerY = ClientSize.Height / 2;
            r = Math.Min(ClientSize.Width, ClientSize.Height) * 0.4f;
            e.Graphics.DrawEllipse(Pens.Black, centerX - r, centerY - r, r + r, r + r);
            //15度一个刻度线
            for(int t = 0; t < 360; t += 30)
            {
                int h = t / 30 - 3;
                if (h <= 0) h = 12+h;
                var angle = t * Math.PI / 180;
                float x =  r * (float)Math.Cos(angle);
                float y =  r * (float)Math.Sin(angle);
                float x1 = centerX - x;
                float y1 = centerY - y;
                float xx = x * 0.9f;
                float yy = y * 0.9f;
                float x2 = centerX - xx;
                float y2 = centerY - yy;
                e.Graphics.DrawLine(Pens.White, x1, y1, x2, y2);
                //e.Graphics.DrawString(h.ToString(), DefaultFont, Brushes.Green, x1, y1);

                //x1 = centerX + x;
                //y1 = centerY + y;
                //x2 = centerX + xx;
                //y2 = centerY + yy;
                //e.Graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
                //e.Graphics.DrawString("+"+t.ToString(), DefaultFont, Brushes.Green, x1, y1);
            }
            {
                //计算角度
                var sec = (DateTime.Now.Second + DateTime.Now.Millisecond / 1000f);
                var min = DateTime.Now.Minute + sec / 60f;
                var hour = DateTime.Now.Hour + min / 60f;
                var secAngle = (-90 + 6 * sec) * Math.PI / 180;
                var minAngle = (-90 + 6 * min) * Math.PI / 180;
                var horAngle = (-90 + 30 * hour) * Math.PI / 180;
                float x1 = centerX + r*0.8f * (float)Math.Cos(secAngle);
                float y1 = centerY + r*0.8f * (float)Math.Sin(secAngle);
                float x2 = centerX + r*0.8f * (float)Math.Cos(minAngle);
                float y2 = centerY + r*0.8f * (float)Math.Sin(minAngle);
                float x3 = centerX + r*0.8f * (float)Math.Cos(horAngle);
                float y3 = centerY + r*0.8f * (float)Math.Sin(horAngle);
                List<PointF> plist = new List<PointF>();
                plist.Add(new PointF(x1, y1));
                plist.Add(new PointF(x2, y2));
                plist.Add(new PointF(x3, y3));
                plist.Add(new PointF(x1, y1));
                e.Graphics.DrawLines(Pens.DarkBlue, plist.ToArray());
                e.Graphics.FillPolygon(Brushes.DarkOrange, plist.ToArray());
            }


        }


    }
}

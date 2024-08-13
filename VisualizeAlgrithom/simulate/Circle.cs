using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace simulate
{
    internal class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public void Draw(Graphics g, Pen pen, Brush brush)
        {
            if (pen != null)
            {
                g.DrawEllipse(Pens.LightYellow, (Center.X - Radius), (Center.Y - Radius), (Radius * 2), (Radius * 2));
            }
            g.FillEllipse(Brushes.Red,Center.X-1,Center.Y-1,2,2);
            if (brush != null)
            {
                g.FillEllipse(brush, Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);
            }
        }

        public bool InCircle(float x, float y)
        {
            Vector2 v = new Vector2(x, y);
            if (Vector2.Distance(v, Center) <= Radius)
            {
                return true;
            }
            return false;
        }


    }
}

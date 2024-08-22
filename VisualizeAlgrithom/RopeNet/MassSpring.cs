using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RopeNet
{
    public class MassPoint
    {
        public float mass;
        public Vector2 position;
        public bool pin;//是否固定
        public Vector2 speed;
        public Vector2 force;
        public Vector2 lastpos;
        public MassPoint(float mass, Vector2 position, bool pin)
        {
            this.mass = mass;
            this.position = position;
            this.pin = pin;
            lastpos = position;
            speed = Vector2.Zero;
            force = Vector2.Zero;
        }
    }

    public class Spring
    {
        public int a;
        public int b;
        public float ks;//劲度系数
        public float restLength;
        public float realLength;

        public Spring(int a, int b, float ks, float rest)
        {
            this.a = a;
            this.b = b;
            this.ks = ks;
            this.restLength = rest;
            this.realLength = rest;
        }
    }


    public class MassSpringSystem
    {
        public List<MassPoint> massPoints = new List<MassPoint>();
        public List<Spring> springs = new List<Spring>();
        const float Ks = 300;
        const float Kd = 0.9f;//内部阻尼
        const float Damping = 0.9f;//空气阻尼

        public void CreateNet()
        {
            const float Ox = 400;
            const float Oy = 400;
            const float Rest = 80;
            const int N = 5;
            massPoints.Add(new MassPoint(10, new Vector2(Ox, Oy), true));
            for (int i = 1; i <= N; i++)
            {
                float t = 2*MathF.PI*i / N;
                float dx = MathF.Cos(t) * Rest;
                float dy = MathF.Sin(t) * Rest;
                massPoints.Add(new MassPoint(10, new Vector2(Ox + dx, Oy + dy), false));
                springs.Add(new Spring(0, i, Ks, Distance(0,i)));
                if (i - 1 > 0)
                {
                    springs.Add(new Spring(i - 1, i, Ks, Distance(i - 1, i)));
                }
            }
            springs.Add(new Spring(1,N, Ks, Distance(1,N)));

        }

        float Distance(int a, int b)
        {
            return Vector2.Distance(massPoints[a].position, massPoints[b].position);
        }

        public void Update()
        {
            float t = 0.1f;
            foreach (MassPoint p in massPoints)
            {
                p.force = Vector2.Zero;
                //空气阻力 f=-kv
                p.force = p.force - p.speed * Damping;
            }

            //弹力
            foreach (Spring s in springs)
            {
                MassPoint Pa = massPoints[s.a];
                MassPoint Pb = massPoints[s.b];
                Vector2 ab = Pb.position - Pa.position;//a->b
                //b给a的力 f=k(ab-|ab|) - kd(vb-va)
                Vector2 Fa = Ks * Vector2.Normalize(ab) * (ab.Length() - s.restLength);
                //内部阻力 kd(vb-va) 速度在ab方向的投影
                Vector2 Fd = Kd * Vector2.Normalize(ab) * (MathF.Abs(Vector2.Dot((Pb.speed - Pa.speed), Vector2.Normalize(ab))));
                Fa = Fa - Fd;
                //a给b的力
                Vector2 Fb = Vector2.Zero - Fa;
                Pa.force = Pa.force + Fa;
                Pb.force = Pb.force + Fb;

            }



            //速度和位移
            foreach (MassPoint p in massPoints)
            {
                if (p.pin)
                {
                    continue;
                }
                Vector2 acc = p.force / p.mass;//加速度
                p.speed = p.speed + acc * t; //下一帧的速度
                //Vector2 pos = p.position + p.speed * t;//下一帧的位置
                Vector2 pos = 2 * p.position - p.lastpos + acc * t * t;//下一帧的位置

                p.lastpos = p.position;
                p.position = pos;

            }
        }

        public List<PointF> GetPoints()
        {
            List<PointF> list = new List<PointF>();
            foreach (MassPoint p in massPoints)
            {
                list.Add(new PointF( p.position));
            }
            return list;
        }

        public List<Spring> GetSprings()
        {
            return springs;
        }


    }
}

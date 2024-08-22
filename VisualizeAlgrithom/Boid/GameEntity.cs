using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace Boid
{
    [Flags]
    enum EBehavior
    {
        Idle = 0,
        Wander = 1,
        Seek = 2,
        Flee = 4,
        Arrive = 8,
        Pursuit = 16,
        Evade = 32,
    }

    class GameEntity
    {
        public int Id;

        public virtual void Update(float timeElapsed) { }
        public virtual void Render(Graphics g) { }
    }

    class Wall : GameEntity
    {
        public Vector2 From;
        public Vector2 To;
        public Vector2 Normal;

        public override void Update(float timeElapsed) { }
        public override void Render(Graphics g) { }
    }



    class Vehicle :GameEntity
    {
        public EBehavior Behavior;
        public Vector2 Pos;
        public Vector2 Force;
        public Vector2 Velocity;
        public Vector2 Heading;
        public Vector2 Side ;

        public float Radius;
        public float Mass;
        public float Speed;
        public float TurnRate;

        public float MaxSpeed;
        public float MaxForce;
        public float MaxTurnRate;

        public Vector2 TargetPos;
        public Vehicle TargetAgent;

        public List<Vector2> Feelers = new List<Vector2>(); //感知方向
        public float FeelDistance = 100; //感知距离
        public float ViewDistance = 100; //视野

        public override void Update(float timeElapsed)
        {
            //计算合力
            Force = CalcForce(timeElapsed);
            //更新速度
            float acc = Force.Length() / Mass;
            Velocity = Velocity + Vector2.Normalize(Force) * acc * timeElapsed;
            Speed = Velocity.Length();
            //更新朝向
            Heading = Vector2.Normalize(Velocity);
            Side = new Vector2(Heading.Y, -Heading.X);
            //更新位置
            Pos = Pos + Velocity * timeElapsed;
        }
        public override void Render(Graphics g)
        {
            g.DrawEllipse(Pens.White, Pos.X - Radius, Pos.Y - Radius, Radius * 2, Radius * 2);
        }

        Vector2 Trunk(Vector2 v, float m)
        {
            float len = MathF.Min(v.Length(), m);
            return Vector2.Normalize(v) * len;
        }

        Vector2 CalcForce(float timeElapsed)
        {
            if(Behavior == EBehavior.Idle)
            {
                return Vector2.Zero;
            }
            Vector2 force = Vector2.Zero;
            if (Behavior.HasFlag(EBehavior.Wander))
            {
                force += Wander();
                force = Trunk(force, MaxForce);
            }

            if (Behavior.HasFlag(EBehavior.Flee))
            {
                force += Flee(TargetPos);
                force = Trunk(force, MaxForce);
            }

            if (Behavior.HasFlag(EBehavior.Seek))
            {
                force += Seek(TargetPos);
                force = Trunk(force, MaxForce);
            }

            if (Behavior.HasFlag(EBehavior.Arrive))
            {
                force += Arrive(TargetPos);
                force = Trunk(force, MaxForce);
            }

            if (Behavior.HasFlag(EBehavior.Pursuit))
            {
                force += Pursuit(TargetAgent);
                force = Trunk(force, MaxForce);
            }

            if (Behavior.HasFlag(EBehavior.Evade))
            {
                force += Evade(TargetAgent);
                force = Trunk(force, MaxForce);
            }

            return force;
        }

        //游荡
        Vector2 Wander()
        {
            return Vector2.Zero;
        }

        //靠近
        Vector2 Seek(Vector2 targetPos)
        {
            Vector2 v = Vector2.Normalize(Pos - targetPos);
            return v * MaxSpeed - Velocity;
        }

        //远离
        Vector2 Flee(Vector2 targetPos)
        {
            Vector2 v = Vector2.Normalize(targetPos - Pos);
            return v * MaxSpeed - Velocity;
        }

        //到达
        Vector2 Arrive(Vector2 targetPos)
        {
            //距离越近速度越小
            Vector2 v = targetPos - Pos;
            float dist = v.Length();
            if(dist < Radius + 2)
            {
                return Vector2.Zero;
            }
            float speed = dist / 0.6f;
            speed=MathF.Min(speed, MaxSpeed);
            return v * speed / dist - Velocity;
        }

        //追赶
        Vector2 Pursuit(Vehicle vehicle)
        {
            float lookAtTime = (vehicle.Pos - Pos).Length() / (MaxSpeed + vehicle.Speed);
            Vector2 targetPos = vehicle.Pos + vehicle.Velocity * lookAtTime;
            return Seek(targetPos);
        }
        //逃离
        Vector2 Evade(Vehicle vehicle)
        {
            float lookAtTime = (vehicle.Pos - Pos).Length() / (MaxSpeed + vehicle.Speed);
            Vector2 targetPos = vehicle.Pos + vehicle.Velocity * lookAtTime;
            return Flee(targetPos);
        }
    }



    class GameWorld
    {
        public List<Vehicle> Vehicles = new List<Vehicle>();
        public List<Wall> Walls = new List<Wall>();
        public Random Randor = new Random();
        public int m_ID = 0;
        public int Width = 1920;
        public int Height = 1080;

        public void CreateWorld()
        {
            for(int i = 0; i < 10; i++)
            {
                Vehicle v = new Vehicle();
                v.Id = ++m_ID;
                v.Behavior = EBehavior.Seek;
                v.Pos = new Vector2(Randor.Next(0, Width), Randor.Next(0, Height));
                v.TurnRate = 0;
                v.MaxTurnRate = 5;
                v.Speed = 0;
                v.MaxSpeed = 10;
                v.Mass = Randor.Next(10, 100);
                v.MaxForce = 300;
                v.Radius = Randor.Next(10, 20);
                
            }
        }

        public void SetTargetPos(Vector2 pos)
        {
            foreach(var vehicle in Vehicles)
            {
                vehicle.TargetPos = pos;
            }
        }

        public void Update(float dt)
        {
            foreach (Wall w in Walls)
            {
                w.Update(dt);
            }

            foreach (var vehicle in Vehicles)
            {
                vehicle.Update(dt);
            }
        }

        public void Render(Graphics g)
        {
            foreach (Wall w in Walls)
            {
                w.Render(g);
            }

            foreach (var vehicle in Vehicles)
            {
                vehicle.Render(g);
            }
        }

    }


}

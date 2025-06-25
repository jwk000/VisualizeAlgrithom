using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace Boid
{

    enum EBehavior
    {
        Idle,
        Wander,
        Seek,
        Flee,
        Arrive,
        Pursuit,
        Evade,
        Boids, //群体行为
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



    class Vehicle : GameEntity
    {
        public EBehavior Behavior;
        public Vector2 Pos;
        public Vector2 Force;
        public Vector2 Velocity;
        public Vector2 Heading;
        public Vector2 Side;

        public float Radius = 10;
        public float Mass = 1f;
        public float Speed = 10;
        public float TurnRate = 5;

        public float MaxSpeed = 100;
        public float MaxForce = 400;
        public float MaxTurnRate = 10;

        public Vector2 TargetPos;
        public Vehicle TargetAgent = null;

        public List<Vector2> Feelers = new List<Vector2>(); //感知方向
        public float FeelDistance = 100; //感知距离
        public float ViewDistance = 100; //视野

        public Pen MyPen = new Pen(Color.WhiteSmoke, 2.0f);

        public override void Update(float dt)
        {
            //计算合力
            Force = CalcForce(dt);
            if (Force.Length() < 0.001f)
            {
                return;
            }
            //更新速度
            float acc = Force.Length() / Mass;
            Velocity = Velocity + Vector2.Normalize(Force) * acc * dt;
            Speed = Velocity.Length();
            if (Speed > MaxSpeed)
            {
                Velocity = Vector2.Normalize(Velocity) * MaxSpeed;
            }
            //更新朝向
            Heading = Vector2.Normalize(Velocity);
            Side = new Vector2(Heading.Y, -Heading.X);
            //更新位置
            Pos = Pos + Velocity * dt;
            // Wrap around
            if (Pos.X > GameWorld.Instance.Width) Pos.X = 0;
            if (Pos.X < 0) Pos.X = GameWorld.Instance.Width;
            if (Pos.Y > GameWorld.Instance.Height) Pos.Y = 0;
            if (Pos.Y < 0) Pos.Y = GameWorld.Instance.Height;
        }
        public override void Render(Graphics g)
        {
            g.DrawEllipse(MyPen, Pos.X - Radius, Pos.Y - Radius, Radius * 2, Radius * 2);
        }

        Vector2 TrunkForce(Vector2 v)
        {
            if (v.Length() < MaxForce)
            {
                return v;
            }

            return Vector2.Normalize(v) * MaxForce;
        }

        Vector2 CalcForce(float timeElapsed)
        {
            Vector2 force = Vector2.Zero;
            switch (Behavior)
            {
                case EBehavior.Wander:
                    force += Wander();
                    break;
                case EBehavior.Flee:
                    force += Flee(TargetPos);
                    break;
                case EBehavior.Seek:
                    force += Seek(TargetPos);
                    break;
                case EBehavior.Arrive:
                    force += Arrive(TargetPos);
                    break;
                case EBehavior.Pursuit:
                    force += Pursuit(TargetAgent);
                    break;
                case EBehavior.Evade:
                    force += Evade(TargetAgent);
                    break;
                case EBehavior.Boids:
                    force += Boids();
                    break;
                default:
                    break;
            }
            force = TrunkForce(force);

            force *= 0.75f; //阻力
            return force;
        }



        //靠近
        Vector2 Seek(Vector2 targetPos)
        {
            Vector2 v = Vector2.Normalize(targetPos - Pos);
            return v * MaxSpeed - Velocity;
        }

        //远离
        Vector2 Flee(Vector2 targetPos)
        {
            Vector2 v = Vector2.Normalize(Pos - targetPos);
            return v * MaxSpeed - Velocity;
        }

        //到达
        Vector2 Arrive(Vector2 targetPos)
        {
            //距离越近速度越小
            Vector2 v = targetPos - Pos;
            float dist = v.Length();
            if (dist < Radius + 2)
            {
                return Vector2.Zero;
            }
            float speed = dist / 0.6f;
            speed = MathF.Min(speed, MaxSpeed);
            return v * speed / dist - Velocity;
        }

        //追赶
        Vector2 Pursuit(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return Vector2.Zero;
            }
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

        //游荡
        Vector2 Wander()
        {
            //游荡是一个随机的方向
            float wanderRadius = 10;
            float wanderAngle = (float)(GameWorld.Instance.Randor.NextDouble() * Math.PI * 2);
            Vector2 wanderForce = new Vector2(
                (float)Math.Cos(wanderAngle) * wanderRadius,
                (float)Math.Sin(wanderAngle) * wanderRadius);
            return wanderForce;
        }

        Vector2 Boids()
        {
            Vector2 force = Vector2.Zero;
            List<Vehicle> vehiclesInView = GameWorld.Instance.GetVehiclesInView(Pos, ViewDistance);
            if (vehiclesInView.Count == 0)
            {
                return force;
            }
            //分离
            foreach (var vehicle in vehiclesInView)
            {
                Vector2 v =Pos - vehicle.Pos;
                if (vehicle != this && v.Length() < Radius * 3)
                {
                    Vector2 F = Vector2.Normalize(v) * MathF.Pow(Radius * 3 - v.Length(), 4);
                    force += F;
                }
            }
            //对齐
            Vector2 avgVelocity = Vector2.Zero;
            foreach (var vehicle in vehiclesInView)
            {
                if (vehicle != this)
                {
                    avgVelocity += vehicle.Velocity;
                }
            }
            if (avgVelocity.Length() > 0)
            {
                avgVelocity /= vehiclesInView.Count;
                force += TrunkForce(avgVelocity - Velocity);
            }
            //聚合
            Vector2 centerOfMass = Vector2.Zero;
            foreach (var vehicle in vehiclesInView)
            {
                if (vehicle != this)
                {
                    centerOfMass += vehicle.Pos;
                }
            }
            if (centerOfMass.Length() > 0)
            {
                centerOfMass /= vehiclesInView.Count;
                force += Seek(centerOfMass);
            }

            Vector2 vt = TargetPos - Pos;
            vt = Vector2.Normalize(vt) * MathF.Pow(vt.Length(),4);
            force += TrunkForce(vt);
            return force;
        }

    }

    class GameWorld
    {
        public static GameWorld Instance = new GameWorld();

        public List<Vehicle> Vehicles = new List<Vehicle>();
        public List<Wall> Walls = new List<Wall>();
        public Random Randor = new Random();
        public int m_ID = 0;
        public int Width = 0;
        public int Height = 0;

        public Vehicle SelectVehicle = null;
        public EBehavior GlobalBehavior = EBehavior.Idle;

        public void CreateWorld()
        {
            for (int i = 0; i < 10; i++)
            {
                Vehicle v = new Vehicle();
                v.Id = ++m_ID;
                v.Behavior = EBehavior.Idle;
                v.Pos = new Vector2(Randor.Next(0, Width), Randor.Next(0, Height));
                Vehicles.Add(v);
            }
        }

        public void AddAgent(Vector2 pos)
        {
            Vehicle v = new Vehicle();
            v.Id = ++m_ID;
            v.Behavior = GlobalBehavior;
            v.Pos = pos;
            Vehicles.Add(v);
        }

        public void SetVehiclesBehavior(EBehavior behavior)
        {
            GlobalBehavior = behavior;
            foreach (var vehicle in Vehicles)
            {
                vehicle.Behavior = behavior;
            }
        }

        public void SetVehiclesTarget(Vehicle vehicle)
        {
            foreach (var v in Vehicles)
            {
                v.TargetAgent = vehicle;
            }
        }

        public List<Vehicle> GetVehiclesInView(Vector2 pos, float viewDistance)
        {
            List<Vehicle> vehiclesInView = new List<Vehicle>();
            foreach (var vehicle in Vehicles)
            {
                if ((vehicle.Pos - pos).Length() <= viewDistance)
                {
                    vehiclesInView.Add(vehicle);
                }
            }
            return vehiclesInView;
        }

        public void SetTargetPos(Vector2 pos)
        {
            foreach (var vehicle in Vehicles)
            {
                vehicle.TargetPos = pos;
            }
        }

        public void SelectAgent(Vector2 pos)
        {

            foreach (var vehicle in Vehicles)
            {
                if ((vehicle.Pos - pos).Length() < vehicle.Radius * 2)
                {
                    SelectVehicle = vehicle;
                    break;
                }
            }

            if (SelectVehicle == null)
            {
                return;
            }

            SelectVehicle.MyPen.Color = Color.Red;
            SetVehiclesTarget(SelectVehicle);
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

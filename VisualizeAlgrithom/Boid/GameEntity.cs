using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Boid
{
    class GameEntity
    {
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public double Scale { get; set; }
        public double Radius { get; set; }

        public virtual void Update(double timeElapsed) { }
        public virtual void Render() { }
    }

    class MovingEntity :GameEntity
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Heading { get; set; }
        public Vector2 Side { get; set; }

        public double Mass;
        public double MaxSpeed;
        public double MaxForce;
        public double MaxTurnRate;

        public override void Update(double timeElapsed)
        {

        }
        public override void Render()
        {
            base.Render();
        }
    }



    class GameWorld
    {

    }

    class SteeringBehaviors
    {
        Vector2 Seek(Vector2 targetPos)
        {

        }

        Vector2 Flee(Vector2 targetPos)
        {

        }

        Vector2 Arrive(Vector2 targetPos)
        {

        }


    }
}

using System.Collections.Generic;
using System.Numerics;

namespace Revolution.ECS.Components
{
    public class MovementComponent : Component
    {
        public int MaxVelocity { get; set; }
        
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        
        public Queue<Vector2> Path { get; set; }
        public Vector2? CurrentTarget { get; set; }

        public MovementComponent()
        {
            Path = new Queue<Vector2>();
        }

        public void Stop()
        {
            VelocityX = 0;
            VelocityY = 0;
            CurrentTarget = null;
            Path.Clear();
        }
    }
}
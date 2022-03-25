namespace Revolution.ECS.Components
{
    public class MovementComponent : Component
    {
        public int MaxVelocity { get; set; }
        
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        
        public int TargetTileDeltaX { get; set; }
        public int TargetTileDeltaY { get; set; }
    }
}
namespace Revolution.ECS.Components
{
    public class MovementComponent : Component
    {
        public int MaxVelocity { get; set; }
        
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        
        public int DestinationTileX { get; set; }
        public int DestinationTileY { get; set; }

        public void Stop()
        {
            VelocityX = 0;
            VelocityY = 0;
        }
    }
}
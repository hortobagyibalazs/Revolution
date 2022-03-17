using Revolution.ECS.Components;

namespace Revolution.ECS.Entities
{
    public class Camera : Entity
    {
        public Camera()
        {
            AddComponent(new CameraComponent());
        }
    }
}
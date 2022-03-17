using Revolution.ECS.Entities;

namespace Revolution.ECS.Components
{
    public class CameraComponent : Component
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Locked { get; private set; }
        public PositionComponent? Target { get; private set; }

        public void LockInPlace(int x, int y)
        {
            X = x;
            y = y;
            LockInPlace();
        }

        public void LockInPlace()
        {
            Locked = true;
        }

        // Entity must have position component
        public void FollowTarget(Entity entity)
        {
            Target = entity.GetComponent<PositionComponent>();
            if (Target == null)
            {
                return;
            }
            LockInPlace();
        }

        public void Unlock()
        {
            Locked = false;
            Target = null;
        }

        public void SnapTo(int x, int y)
        {
            if (!Locked)
            {
                X = x;
                Y = y;
            }
        }
    }
}
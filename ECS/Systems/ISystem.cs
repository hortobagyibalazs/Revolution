using Revolution.ECS.Components;

namespace Revolution.ECS.Systems
{
    public interface ISystem
    {
        void Update(int deltaMs);
    }
}
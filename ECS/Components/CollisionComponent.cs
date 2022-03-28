
using System.Windows;

namespace Revolution.ECS.Components
{
    public class CollisionComponent : Component
    {
        private SizeComponent _sizeComponent;
        private PositionComponent _positionComponent;

        public CollisionComponent(SizeComponent sizeComponent, PositionComponent positionComponent)
        {
            _sizeComponent = sizeComponent;
            _positionComponent = positionComponent;
        }

        public bool CollidesWith(CollisionComponent component)
        {
            return 
                new Rect(_positionComponent.X, _positionComponent.Y, _sizeComponent.Width, _sizeComponent.Height)
                .IntersectsWith(
                    new Rect(component._positionComponent.X, component._positionComponent.Y,
                        component._sizeComponent.Width, component._sizeComponent.Height)
                );
        }
    }
}
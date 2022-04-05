
using System.Windows;

namespace Revolution.ECS.Components
{
    public class CollisionComponent : Component
    {
        private GameMapObjectComponent _mapObjectComp;

        public CollisionComponent(GameMapObjectComponent mapObjectComponent)
        {
            _mapObjectComp = mapObjectComponent;
        }

        public bool CollidesWith(CollisionComponent component)
        {
            return 
                new Rect(_mapObjectComp.X, _mapObjectComp.Y, _mapObjectComp.Width, _mapObjectComp.Height)
                .IntersectsWith(
                    new Rect(component._mapObjectComp.X, component._mapObjectComp.Y,
                        component._mapObjectComp.Width, component._mapObjectComp.Height)
                );
        }
    }
}
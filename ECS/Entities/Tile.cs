using Avalonia.Controls;
using Avalonia.Media;
using Revolution.ECS.Components;

namespace Revolution.ECS.Entities
{
    public class Tile : Entity
    {
        public Tile()
        {
            AddComponent(new PositionComponent());
            AddComponent(new RenderComponent() {Renderable = new Image()});
            AddComponent(new SizeComponent() {Width = 16, Height = 16});
        }
    }
}
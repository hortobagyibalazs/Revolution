using Avalonia;
using Avalonia.Controls;

namespace Revolution.ECS.Components
{
    public class RenderComponent : Component
    {
        public virtual Control Renderable { get; set; }
    }
}
using System.Windows;
using System.Windows.Controls;

namespace Revolution.ECS.Components
{
    public class RenderComponent : Component
    {
        public virtual FrameworkElement Renderable { get; set; }
        public virtual int ZIndex {
            set
            {
                Panel.SetZIndex(Renderable, value);
            }

            get
            {
                return Panel.GetZIndex(Renderable);
            }
        }
    }
}
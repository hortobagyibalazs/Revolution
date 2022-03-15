using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace Revolution.ECS.Components
{
    public class SpriteComponent : RenderComponent
    {
        private Image img = new Image();

        private string _src;
        public string Source
        {
            get => _src;
            set => SetSource(value);
        }

        public override Control Renderable
        {
            get => img;
        }

        private void SetSource(string src)
        {
            _src = src;
            img.Source = new Bitmap(src);
        }
    }
}
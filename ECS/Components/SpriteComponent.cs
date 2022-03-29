using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Revolution.ECS.Components
{
    public class SpriteComponent : RenderComponent
    {
        private System.Windows.Controls.Image img = new System.Windows.Controls.Image();

        private string _src;
        public string Source
        {
            get => _src;
            set => SetSource(value);
        }

        public override FrameworkElement Renderable
        {
            get => img;
        }

        private void SetSource(string src)
        {
            _src = src;
            img.Source = new BitmapImage(new Uri(src, UriKind.Relative));
        }
    }
}
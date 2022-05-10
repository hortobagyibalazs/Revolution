using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Revolution.ECS.Components
{
    public class SpriteComponent : RenderComponent
    {
        private Image img;
        private Uri _src;
        public virtual Uri Source
        {
            get => _src;
            set => SetSource(value);
        }

        public override FrameworkElement Renderable
        {
            get
            {
                return img;
            }
        }

        public SpriteComponent()
        {
            img = new Image();
        }

        private void SetSource(Uri src)
        {
            if (src == null)
            {
                ((Image)Renderable).Source = null;
                return;
            }

            _src = src;
            var bitmap = new BitmapImage(src);
            ((Image)Renderable).Source = bitmap;
        }
    }
}
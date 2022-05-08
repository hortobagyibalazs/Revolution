using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Revolution.HUD
{
    public class HudMiniActionButton : Button
    {
        private Action _action;
        private Action _hover;
        private Action _leave;
        public bool IsActive { get; set; }

        public HudMiniActionButton(Uri src, Action click, Action hover = null, Action leave = null)
        {
            Content = new Image()
            {
                Source = new BitmapImage(src)
            };
            IsActive = true;
            Margin = new System.Windows.Thickness(1);

            _action = click;
            _hover = hover;
            _leave = leave;
            this.Click += HudMiniActionButton_Click;
            this.MouseMove += HudMiniActionButton_MouseMove;
            this.MouseLeave += HudMiniActionButton_MouseLeave;
        }

        private void HudMiniActionButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _leave?.Invoke();
        }

        private void HudMiniActionButton_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _hover?.Invoke();
        }

        private void HudMiniActionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsActive)
            {
                _action.Invoke();
            }
        }
    }
}

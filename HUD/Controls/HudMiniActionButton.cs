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
        public bool IsActive { get; set; }

        public HudMiniActionButton(Uri src, Action fun)
        {
            Content = new Image()
            {
                Source = new BitmapImage(src)
            };
            IsActive = true;
            Margin = new System.Windows.Thickness(1);

            _action = fun;
            this.Click += HudMiniActionButton_Click;
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

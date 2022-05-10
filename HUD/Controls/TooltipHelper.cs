using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Revolution.HUD.Controls
{
    public class TooltipHelper
    {
        public static FrameworkElement GetBasicTooltipForEntityPurchase(string name, int gold, int wood)
        {
            var buildingNameLabel = new Label();
            buildingNameLabel.Foreground = Brushes.White;
            buildingNameLabel.Content = name;

            var goldIcon = new Image() { Source = new BitmapImage(new Uri(@"\Assets\Images\spr_gold_coin_gui.png", UriKind.Relative)), Width = 20, Height = 20 };
            var woodIcon = new Image() { Source = new BitmapImage(new Uri(@"\Assets\Images\spr_tree_gui.png", UriKind.Relative)), Width = 20, Height = 20 };

            var goldLabel = new Label() { Content = gold, Foreground = Brushes.White };
            var woodLabel = new Label() { Content = wood, Foreground = Brushes.White };

            var resourceListView = new WrapPanel();
            resourceListView.Children.Add(woodIcon);
            resourceListView.Children.Add(woodLabel);
            resourceListView.Children.Add(goldIcon);
            resourceListView.Children.Add(goldLabel);

            var contentHolderView = new DockPanel();
            contentHolderView.Margin = new Thickness(16);
            contentHolderView.Children.Add(buildingNameLabel);
            contentHolderView.Children.Add(resourceListView);
            DockPanel.SetDock(buildingNameLabel, Dock.Top);
            DockPanel.SetDock(resourceListView, Dock.Bottom);

            return contentHolderView;
        }
    }
}

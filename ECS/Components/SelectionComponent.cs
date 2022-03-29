using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Revolution.ECS.Components
{
    public class SelectionComponent : RenderComponent, INotifyPropertyChanged
    {
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged();
                }
            }
        }

        public override FrameworkElement Renderable { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SelectionComponent()
        {
            Renderable = new Border()
            {
                BorderBrush = new SolidColorBrush(System.Windows.Media.Brushes.Yellow.Color),
                BorderThickness = new System.Windows.Thickness(2),
            };
            ZIndex = 0;
            OnPropertyChanged();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (_selected)
            {
                Renderable.Visibility = Visibility.Visible;
            }
            else
            {
                Renderable.Visibility = Visibility.Hidden;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

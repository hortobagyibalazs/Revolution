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

        private PositionComponent posComp;
        private SizeComponent sizeComp;

        public SelectionComponent(PositionComponent posComp, SizeComponent sizeComp)
        {
            this.posComp = posComp;
            this.sizeComp = sizeComp;

            posComp.PropertyChanged += PosComp_PropertyChanged;
            sizeComp.PropertyChanged += SizeComp_PropertyChanged;

            Renderable = new Border()
            {
                BorderBrush = new SolidColorBrush(System.Windows.Media.Brushes.Yellow.Color),
                BorderThickness = new System.Windows.Thickness(1),
            };
            ZIndex = 0;
            OnPropertyChanged();
        }

        private void SizeComp_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Renderable.Width = sizeComp.Width;
            Renderable.Height = sizeComp.Height;
        }

        private void PosComp_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Canvas.SetLeft(Renderable, posComp.X);
            Canvas.SetTop(Renderable, posComp.Y);
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

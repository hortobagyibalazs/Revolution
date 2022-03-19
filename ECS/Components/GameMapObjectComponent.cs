using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Revolution.ECS.Components
{
    // Represents an object placed on the map
    public class GameMapObjectComponent : Component, INotifyPropertyChanged
    {
        /* NOTE : These values differ from [SizeComponent] & [PositionComponent] values, which are on screen coordinates */
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public int X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }
        public int Y 
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }
        public int Width         
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public int Height 
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
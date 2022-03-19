using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Revolution.IO;

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

        // event handler util variables
        private int _newvalue;
        private int _oldvalue;

        public int X
        {
            get => _x;
            set
            {
                if (_x == value) return;
                
                _oldvalue = _x;
                _newvalue = value;
                _x = value;
                OnPropertyChanged();
            }
        }
        public int Y 
        {
            get => _y;
            set
            {
                if (_y == value) return;

                _oldvalue = _y;
                _newvalue = value;
                _y = value;
                OnPropertyChanged();
            }
        }
        public int Width         
        {
            get => _width;
            set
            {
                if (_width == value) return;

                _oldvalue = _width;
                _newvalue = value;
                _width = value;
                OnPropertyChanged();
            }
        }
        public int Height 
        {
            get => _height;
            set
            {
                if (_height == value) return;

                _oldvalue = _height;
                _newvalue = value;
                _height = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventExtendedArgs(propertyName, _oldvalue, _newvalue));
        }
    }
}
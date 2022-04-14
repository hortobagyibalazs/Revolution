using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private int _oldx;
        private int _oldy;
        private int _oldwidth;
        private int _oldheight;

        public int X
        {
            get => _x;
            set
            {
                if (_x == value) return;

                SetOldVariables();
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

                SetOldVariables();
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

                SetOldVariables();
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

                SetOldVariables();
                _height = value;
                OnPropertyChanged();
            }
        }
        private void SetOldVariables()
        {
            _oldx = _x;
            _oldy = _y;
            _oldwidth = _width;
            _oldheight = _height;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventGameMapObjectArgs(propertyName, _oldx, _x, _oldy, _y, _oldwidth, _width, _oldheight, _height));
        }
    }

    public class PropertyChangedEventGameMapObjectArgs : PropertyChangedEventArgs
    {
        public virtual int OldX { get; private set; }
        public virtual int NewX { get; private set; }
        public virtual int OldY { get; private set; }
        public virtual int NewY { get; private set; }
        public virtual int OldWidth { get; private set; }
        public virtual int NewWidth { get; private set; }
        public virtual int OldHeight { get; private set; }
        public virtual int NewHeight { get; private set; }

        public PropertyChangedEventGameMapObjectArgs(string propertyName, int oldx, int newx, int oldy, int newy, int oldwidth, int newwidth, int oldheight, int newheight)
            : base(propertyName)
        {
            OldX = oldx;
            OldY = oldy;
            OldWidth = oldwidth;
            OldHeight = oldheight;

            NewX = newx;
            NewY = newy;
            NewWidth = newwidth;
            NewHeight = newheight;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revolution.ECS.Components
{
    public enum Direction
    {
        Left,
        Right
    }

    public class DirectionComponent : Component, INotifyPropertyChanged
    {
        private Direction _direction = Direction.Right;
        public Direction Direction
        { 
            get
            { 
                return _direction; 
            }
            set
            {
                if (value != _direction)
                {
                    _direction = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(null));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public override void CleanUp()
        {
            base.CleanUp();

            PropertyChanged = null;
        }
    }
}

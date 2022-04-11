using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Revolution.ECS.Components
{
    public class MovementComponent : Component, INotifyPropertyChanged
    {
        private int velocity_x;
        private int velocity_y;

        public int MaxVelocity { get; set; }
        
        public int VelocityX
        {
            get
            {
                return velocity_x;
            }
            set
            {
                if (velocity_x == value) return;
                velocity_x = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }
        public int VelocityY
        {
            get
            {
                return velocity_y;
            }
            set
            {
                if (velocity_y == value) return;
                velocity_y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public Queue<Vector2> Path { get; set; }
        public Vector2? CurrentTarget { get; set; }

        public MovementComponent()
        {
            Path = new Queue<Vector2>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Stop()
        {
            VelocityX = 0;
            VelocityY = 0;
            CurrentTarget = null;
            Path.Clear();
        }
    }
}
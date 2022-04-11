using System.ComponentModel;
using System.Runtime.CompilerServices;
using Revolution.ECS.Entities;

namespace Revolution.ECS.Components
{
    public enum BuildingState
    {
        /**
         * Still looking for place on the map for building
         */
        Placing,
        
        /**
         * Building hasn't been built yet
         */
        UnderConstruction,
        
        /**
         * Building is in its final form
         */
        Built,

        /*
         * This is a temporary state for displaying destroyed building sprite
         */
        Destroyed
    }

    public class BuildingComponent : Component, INotifyPropertyChanged
    {
        private BuildingState _state = BuildingState.UnderConstruction;

        public BuildingState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
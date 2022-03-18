using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Revolution.ECS.Entities;

namespace Revolution.ECS.Components
{
    public enum BuildingState
    {
        UnderConstruction,
        Built
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System.ComponentModel;

namespace Revolution.IO
{
    public class PropertyChangedEventExtendedArgs : PropertyChangedEventArgs
    {
        public virtual object OldValue { get; private set; }
        public virtual object NewValue { get; private set; }

        public PropertyChangedEventExtendedArgs(string propertyName, object oldValue, 
            object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
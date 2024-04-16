using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFUsefullThings
{
    public class INotifyPropertyChangedPlus : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<PropertyChangedEventHandler> _Subscribers = new List<PropertyChangedEventHandler>();

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual bool SetWithAction<T>(ref T field, T value,
            Action refreshAction, [CallerMemberName] string propertyName = null)
        {
            var result = Set(ref field, value, propertyName);
            refreshAction();
            return result;
        }

        public void SubscribeToPropertyChanged(PropertyChangedEventHandler handler)
        {
            PropertyChanged += handler;
            _Subscribers.Add(handler);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _Disposed;

        public virtual void Dispose(bool disposing)
        {
            if (_Disposed) return;

            if (disposing)
            {
                foreach (var subscriber in _Subscribers)
                {
                    PropertyChanged -= subscriber;
                }
                _Subscribers.Clear();
            }

            _Disposed = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFUsefullThings;

namespace WPFUsefullThings.ViewModels
{
    public class ItemViewModel<T> : INotifyPropertyChangedPlus
        where T : class, IUpdateable<T>, INotifyPropertyChanged, new()
    {
        private ObjectView<T> _item;
        public ObjectView<T> Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        public DynamicIsValid IsValid => Item.IsPropertyValid;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ItemViewModel(T item, ObservableCollection<T> itemCollection, Type contextType) : this()
        {
            Item = new ObjectView<T>(item, itemCollection, contextType);
        }

        public ItemViewModel(ObservableCollection<T> itemCollection, Type contextType) : this()
        {
            Item = new ObjectView<T>(itemCollection, contextType);
        }

        public ItemViewModel()
        {
            SaveCommand = new RelayCommand(obj => ExecuteSaveCommand(obj), obj => Item.IsValid);
            CancelCommand = new RelayCommand(obj => CloseWindow(obj));
        }

        private void ExecuteSaveCommand(object obj)
        {
            Item.Save();
            CloseWindow(obj);
        }

        internal void CloseWindow(object obj) => (obj as Window).Close();
    }
}

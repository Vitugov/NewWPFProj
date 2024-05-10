using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFUsefullThings.ViewModels;

namespace WPFUsefullThings
{
    public class ItemViewModel<T> : INotifyPropertyChangedPlus, IItemViewModel
        where T : ProjectModel, new()
    {
        public string Header { get; set; }

        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> Dic => Item.Dic;
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> SubCollectionDic
            => Item.SubCollectionDic;

        private ObjectView<T> _item;
        public ObjectView<T> Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        private ProjectModel _selectedCollectionItem;
        public ProjectModel SelectedCollectionItem
        {
            get => _selectedCollectionItem;
            set => Set(ref _selectedCollectionItem, value);
        }

        public DynamicIsValid IsValid => Item.IsPropertyValid;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ItemViewModel(T? item, ObservableCollection<T> itemCollection) : this()
        {

            var addition = item == null ? "*" : "";
            
            Header = typeof(T).GetClassOverview().DisplayNameSingular + addition;
            Item = new ObjectView<T>(item, itemCollection);
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

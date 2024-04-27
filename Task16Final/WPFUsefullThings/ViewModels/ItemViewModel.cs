using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WPFUsefullThings;

namespace WPFUsefullThings
{
    public class ItemViewModel<T> : INotifyPropertyChangedPlus
        where T : ProjectModel, new()
    {
        private DbContext GetContext() => (DbContext)Activator.CreateInstance(_dbContextType);
        private readonly Type _dbContextType;

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

        public ItemViewModel(T? item, ObservableCollection<T> itemCollection, Type contextType) : this()
        {
            _dbContextType = contextType;
            var addition = item == null ? "*" : "";
            
            Header = ClassOverview.Dic[typeof(T).Name].DisplayNameSingular + addition;
            Item = new ObjectView<T>(item, itemCollection, contextType);
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

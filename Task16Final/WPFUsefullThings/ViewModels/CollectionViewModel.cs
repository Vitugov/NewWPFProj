using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFUsefullThings
{
    public class CollectionViewModel<T> : INotifyPropertyChangedPlus
        where T : ProjectModel, new()
    {        
        public string Header { get; set; }
        
        private ObservableCollection<T> _itemCollection;
        public ObservableCollection<T> ItemCollection
        {
            get => _itemCollection;
            set => Set(ref _itemCollection, value);
        }

        private ListCollectionView _itemCollectionView;

        public ListCollectionView ItemCollectionView
        {
            get => _itemCollectionView;
            set => Set(ref _itemCollectionView, value);
        }


        private T? _selectedItem;
        public T? SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private string _filterText = "";
        public string FilterText
        {
            get => _filterText;
            set
            {
                Set(ref _filterText, value);
                ItemCollectionView.Filter = obj => obj.IsContainingString(FilterText);
            }
        }

        public ICommand AddNewItemCommand { get; }
        public ICommand ChangeItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public CollectionViewModel()
        {
            Header = typeof(T).GetClassOverview().DisplayNamePlural;
            ItemCollection = [.. DbHandler.GetShallowSetList<T>()];
            ItemCollectionView = new ListCollectionView(ItemCollection);
            ItemCollectionView.SortDescriptions.Add(new SortDescription("DisplayName", ListSortDirection.Ascending));
            
            AddNewItemCommand = new RelayCommand(obj => ExecuteChangeItem());
            ChangeItemCommand = new RelayCommand(obj => ExecuteChangeItem(SelectedItem), obj => SelectedItem != null);
            DeleteItemCommand = new RelayCommand(obj => ExecuteDeleteItem(SelectedItem), obj => SelectedItem != null);
        }

        private void ExecuteChangeItem(T? item = null)
        {
            var itemVM = new ItemViewModel<T>(item, ItemCollection);
            var itemView = new ItemWindow<T>(itemVM);
            itemView.ShowDialog();
        }

        private void ExecuteDeleteItem(T? item)
        {
            if (item == null) { return; }
            var list = item.GetLinksOnItem();
            if (list.Any())
            {
                MessageBox.Show($"Невозможно удалить объект. На данный объект есть {list.Count} ссылок в базе данных.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ItemCollection.Remove(item);
            item.DeleteItem();
        }
    }
}

using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WPFUsefullThings.ViewModels
{
    public class CollectionViewModel<T> : INotifyPropertyChangedPlus
        where T : class, IUpdateable<T>, INotifyPropertyChanged, new()
    {
        private DbContext GetContext() => (DbContext)Activator.CreateInstance(_contextType);
        private readonly Type _contextType;
        private readonly PropertyInfo[] _itemProperties;
    
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
                ItemCollectionView.Filter = obj => DoesObjectContainString(obj, FilterText);
            }
        }

        public ICommand AddNewItemCommand { get; }
        public ICommand ChangeItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public CollectionViewModel(Type contextType, T obj)
        {
            _contextType = contextType;
            _itemProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var context = GetContext())
            {
                ItemCollection = new ObservableCollection<T>(context.Set<T>());
            }

            ItemCollectionView = new ListCollectionView(ItemCollection);

            AddNewItemCommand = new RelayCommand(obj => ExecuteChangeItem());
            ChangeItemCommand = new RelayCommand(obj => ExecuteChangeItem(SelectedItem), obj => SelectedItem != null);
            DeleteItemCommand = new RelayCommand(obj => ExecuteDeleteItem(SelectedItem), obj => SelectedItem != null);
        }

        private bool DoesObjectContainString(object obj, string str)
        {;
            if (str == null || str == "")
                return true;
            var result = _itemProperties.Any(property => property.GetValue(obj).ToString().Contains(str));
            return result;
        }

        private void RefreshClients()
        {
            using (var context = GetContext())
            {
                ItemCollection = new ObservableCollection<T>(context.Set<T>());
            }
        }

        private void ExecuteChangeItem(T? item = null)
        {
            var clientVM = new ClientVM(item);
            var clientView = new ClientView(clientVM);
            clientView.ShowDialog();
        }

        private void ExecuteDeleteItem(T? item)
        {
            if (item == null)
            {
                return;
            }

            ItemCollection.Remove(item);

            using (var context = GetContext())
            {
                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }

        private void MainWindowVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(SelectedItem) || e.PropertyName == nameof(FilterText))
            //{
            //    RefreshOrders();
            //}
            //if (e.PropertyName == nameof(Orders))
            //{
            //    ItemCollectionView = new ListCollectionView(Orders);
            //}
        }
    }
}

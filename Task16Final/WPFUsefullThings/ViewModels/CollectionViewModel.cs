﻿using Microsoft.SqlServer.Management.Sdk.Sfc;
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

namespace WPFUsefullThings
{
    public class CollectionViewModel<T> : INotifyPropertyChangedPlus
        where T : ProjectModel, new()
    {
        private DbContext GetContext() => (DbContext)Activator.CreateInstance(_dbContextType);
        private readonly Type _dbContextType;

        private readonly ProjectModel? _parent;
        
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

        public CollectionViewModel(ProjectModel parent, IEnumerable<ProjectModel> list, Type dbContextType) : this()
        {
            _dbContextType = dbContextType;
            Header = ClassOverview.Dic[typeof(T).Name].DisplayNamePlural;
            _parent = parent;

            var table = list.Select(obj => obj.Id).ToList();

            using (var context = GetContext())
            {
                var query = context.GetDeepData<T>().Where(o => table.Contains(o.Id));
                ItemCollection = new ObservableCollection<T>(query);
            }

            ItemCollectionView = new ListCollectionView(ItemCollection);
        }

        public CollectionViewModel(Type dbContextType) : this()
        {
            _dbContextType = dbContextType;
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            Header = classOverview.DisplayNamePlural;

            var context = GetContext();
            ItemCollection = context.GetDeepData<T>();

            ItemCollectionView = new ListCollectionView(ItemCollection);
        }

        public CollectionViewModel()
        {
            AddNewItemCommand = new RelayCommand(obj => ExecuteChangeItem());
            ChangeItemCommand = new RelayCommand(obj => ExecuteChangeItem(SelectedItem), obj => SelectedItem != null);
            DeleteItemCommand = new RelayCommand(obj => ExecuteDeleteItem(SelectedItem), obj => SelectedItem != null);
        }

        private void ExecuteChangeItem(T? item = null)
        {
            var itemVM = new ItemViewModel<T>(item, ItemCollection, _dbContextType);
            var itemView = new ItemWindow(typeof(T), itemVM);
            itemView.ShowDialog();
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
    }
}

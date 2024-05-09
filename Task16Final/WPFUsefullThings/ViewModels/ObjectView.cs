using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Collections;

namespace WPFUsefullThings
{
    public class ObjectView<T> : INotifyPropertyChanged
        where T : ProjectModel, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public T Edit
        {   get;
            set; }
        public DynamicIsValid IsPropertyValid => _validation.IsValid;
        public bool IsValid => _validation.Validate();

        private Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> _dic;
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> Dic { get; set; }

        private Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> _subCollectionDic;
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> SubCollectionDic { get; set; } = [];

        private readonly ObservableCollection<T> _collection;
        private readonly Type _dbContextType;
        private readonly T _original;
        private readonly bool _isNew = false;
        private readonly Validation<T> _validation;
        private readonly ClassOverview _classOverview;


        public ObjectView(T? original, ObservableCollection<T> collection, Type contextType)
        {
            IfNotADbContextThrowExeption(contextType);
            _dbContextType = contextType;
            _classOverview = ClassOverview.Dic[typeof(T).Name];
            Dic = DbContextCreator.Create().GetDictionariesOfRelatedProperties(typeof(T));
            
            if (original == null)
            {
                _isNew = true;
                _original = new T();
            }
            else
            {
                using (var context = DbContextCreator.Create())
                {
                    var query = context.DeepSet<T>().Where(e => e.Id == original.Id);
                    _original = query.First();
                }

            }
            if (_classOverview.HaveCollection)
            {
                var collectionGenericType = _classOverview.CollectionGenericParameter;
                SubCollectionDic = DbContextCreator.Create().GetDictionariesOfRelatedProperties(collectionGenericType);
            }

            Edit = (T)_original.Clone();
            _collection = collection;
            _validation = new Validation<T>(Edit);
        }

        public void Save()
        {
            if (!IsValid)
            {
                ValidationRules.ShowErrorMessage();
                return;
            }
            if (_classOverview.HaveSubCollection)
            {
                SaveSubCollection();
            }
            _original.UpdateFrom(Edit);
            
            using (var context = DbContextCreator.Create())
            {
                var dbSet = context.Set<T>();
                dbSet.Update(_original);
                context.Entry(_original).State = EntityState.Added;
                context.SaveChanges();
            }
            
            if (_isNew)
            {
                _collection.Add(_original);
            }
        }

        private void IfNotADbContextThrowExeption(Type contextType)
        {
            if (contextType != typeof(DbContext) && !contextType.IsSubclassOf(typeof(DbContext)))
                throw new ArgumentException("Type of the context don't belong to class DbContext");
        }

        //private IList GetSubCollectionElementsToDelete(T edit, T origin)
        //{
        //        IList listToDelete = CreateListOfType(_classOverview.CollectionGenericParameter);
        //        var originalCollection = _classOverview.GetCollectionFor(origin);
        //        var editCollection = _classOverview.GetCollectionFor(edit);
        //        foreach (var item in originalCollection)
        //        {
        //            if (!editCollection.Contains(item))
        //            {
        //                listToDelete.Add(item);
        //            }
        //        }
        //        return listToDelete;
        //}

        //private IList GetSubCollectionElementsToAdd(T edit, T origin)
        //{
        //    IList listToDelete = CreateListOfType(_classOverview.CollectionGenericParameter);
        //    var originalCollection = _classOverview.GetCollectionFor(origin);
        //    var editCollection = _classOverview.GetCollectionFor(edit);
        //    foreach (var item in originalCollection)
        //    {
        //        if (!editCollection.Contains(item))
        //        {
        //            listToDelete.Add(item);
        //        }
        //    }
        //    return listToDelete;
        //}

        private static IList CreateListOfType(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private void SaveSubCollection()
        {
            Type genericListType = typeof(SubCollectionSaver<>).MakeGenericType(_classOverview.CollectionGenericParameter);
            Activator.CreateInstance(genericListType, _classOverview.GetCollectionFor(Edit), _classOverview.GetCollectionFor(_original));
        }
    }
}

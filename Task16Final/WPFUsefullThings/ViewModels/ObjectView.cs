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


        public ObjectView(T? original, ObservableCollection<T> collection, Type contextType)
        {
            if (original == null)
            {
                _isNew = true;
                original = new T();
            }
            IfNotADbContextThrowExeption(contextType);
            _dbContextType = contextType;
            var context = (DbContext)Activator.CreateInstance(_dbContextType);
            Dic = context.GetDictionariesOfRelatedProperties(typeof(T));
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            if (classOverview.HaveCollection)
            {
                var collectionGenericType = ClassOverview.Dic[typeof(T).Name].CollectionGenericParameter;
                SubCollectionDic = context.GetDictionariesOfRelatedProperties(collectionGenericType);
            }

            _original = original;
            Edit = (T)original.Clone();
            _collection = collection;
            _validation = new Validation<T>(Edit);

            //foreach (var property in Dic.Keys)
            //{
            //    var id = ((ProjectModel)typeof(T).GetProperty(property).GetValue(Edit)).Id;
            //    var obj = Dic[property].Where(pair => pair.Value.Id == id).Select(pair => pair.Value).FirstOrDefault();
            //    typeof(T).GetProperty(property).SetValue(Edit, obj);
            //}
        }

        public void Save()
        {
            if (!IsValid)
            {
                ValidationRules.ShowErrorMessage();
                return;
            }

            _original.UpdateFrom(Edit);
            
            using (var context = GetContext())
            {
                var dbSet = context.Set<T>();
                dbSet.Update(_original);
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

        private DbContext GetContext() => (DbContext)Activator.CreateInstance(_dbContextType);
    }
}

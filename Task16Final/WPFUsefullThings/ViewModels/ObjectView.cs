using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFUsefullThings
{
    public class ObjectView<T> : INotifyPropertyChanged
        where T : ProjectModel, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public T Edit { get; set; }
        public DynamicIsValid IsPropertyValid => _validation.IsValid;
        public bool IsValid => _validation.Validate();

        private Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> _dic;
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> Dic { get; set; }

        private Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> _subCollectionDic;
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> SubCollectionDic { get; set; } = [];

        private readonly ObservableCollection<T> _collection;
        private readonly T _original;
        private readonly bool _isNew = false;
        private readonly Validation<T> _validation;
        private readonly ClassOverview _classOverview;


        public ObjectView(T? original, ObservableCollection<T> collection)
        {
            _classOverview = typeof(T).GetClassOverview();
            Dic = typeof(T).GetDictionariesOfRelatedProperties();
            
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
                SubCollectionDic = collectionGenericType.GetDictionariesOfRelatedProperties();
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
                if (_isNew)
                {
                    context.Entry(_original).State = EntityState.Added;
                }
                context.SaveChanges();
            }
            
            if (!_isNew)
            {
                _collection.Remove(_original);
            }
            _collection.Add(_original);
        }

        private void IfNotADbContextThrowExeption(Type contextType)
        {
            if (contextType != typeof(DbContext) && !contextType.IsSubclassOf(typeof(DbContext)))
                throw new ArgumentException("Type of the context don't belong to class DbContext");
        }

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

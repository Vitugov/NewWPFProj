using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;

namespace WPFUsefullThings
{
    public class SubCollectionSaver<T>
        where T : ProjectModel
    {
        public List<T> ToAdd { get; set; }
        public List<T> ToUpdate { get; set; }
        public List<T> ToDelete { get; set; }
        private ObservableCollection<T> _editCollection { get; set; }
        private ObservableCollection<T> _originCollection { get; set; }

        public SubCollectionSaver(IList editCollection, IList originColection) 
        {
            _editCollection = (ObservableCollection<T>)editCollection;
            _originCollection = (ObservableCollection<T>)originColection;
            ToAdd = _editCollection
                .Where(item => !_originCollection.Contains(item))
                .ToList();
            ToUpdate = _originCollection
                .Where(item => _editCollection.Contains(item))
                .ToList();
            ToDelete = _originCollection
                .Where(item => !_editCollection.Contains(item))
                .ToList();
            Save();
        }

        public void Save()
        {
            using (var DbContext = DbContextCreator.Create())
            {
                var dbSet = DbContext.Set<T>();
                dbSet.RemoveRange(ToDelete);
                ToAdd.ForEach(item => DbContext.Entry(item).State = EntityState.Added);
                dbSet.UpdateRange(ToUpdate);
                DbContext.SaveChanges();
            }
        }

    }
}

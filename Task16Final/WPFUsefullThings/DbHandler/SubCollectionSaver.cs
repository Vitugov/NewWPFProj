using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class SubCollectionSaver<T>
        where T : ProjectModel
    {
        public List<T> ToAdd { get; set; }
        public List<T> ToUpdate { get; set; }
        public List<T> ToDelete { get; set; }
        public ObservableCollection<T> EditCollection { get; set; }
        public ObservableCollection<T> OriginCollection { get; set; }

        public SubCollectionSaver(IList editCollection, IList originColection) 
        {
            EditCollection = (ObservableCollection<T>)editCollection;
            OriginCollection = (ObservableCollection<T>)originColection;
            ToAdd = EditCollection
                .Where(item => !OriginCollection.Contains(item))
                .ToList();
            ToUpdate = OriginCollection
                .Where(item => EditCollection.Contains(item))
                .ToList();
            ToDelete = OriginCollection
                .Where(item => !EditCollection.Contains(item))
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

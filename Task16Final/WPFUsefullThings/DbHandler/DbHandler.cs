using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFUsefullThings
{
    public class DbHandler
    {
        public static List<ProjectModel> GetSet(Type type)
        {
            using (var context = DbContextCreator.Create())
            {
                return context.Set(type).ToList();
            }
        }

        public static T GetDeepSetForObj<T>(T obj)
            where T : ProjectModel
        {
            using (var context = DbContextCreator.Create())
            {
                var query = context.DeepSet<T>().Where(e => e.Id == obj.Id);
                return query.First();
            }
        }

        public static List<T> GetShallowSetList<T>()
            where T : ProjectModel
        {
            using (var context = DbContextCreator.Create())
            {
                var query = context.ShallowSet<T>();
                return query.ToList();
            }
        }

        public static void DeleteItem<T>(T item)
            where T : ProjectModel
        {
            var classOverview = typeof(T).GetClassOverview();
            var userSubCollections = classOverview.CollectionProperties
                .Where(property => property.IsGenericClassSubClass);
            using (var context = DbContextCreator.Create())
            {
                var query = context.DeepSet<T>().Where(e => e.Id == item.Id);
                item = query.First();
                foreach (var collectionProperty in userSubCollections)
                {
                    var collection = ClassOverview.GetCollectionFor(collectionProperty, item);
                    foreach (var row in collection)
                    {
                        context.Entry(row).State = EntityState.Deleted;
                    }
                }
                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }

        public static void SaveItem<T>(T edit, T original, bool isNew)
            where T : ProjectModel
        {
            //var classOverview = typeof(T).GetClassOverview();
            //if (classOverview.HaveSubCollection)
            //{
                ClassOverview.InvokeSaver(typeof(SubCollectionSaver<>), edit, original);
            //}
            
            original.UpdateFrom(edit);

            using (var context = DbContextCreator.Create())
            {
                var dbSet = context.Set<T>();
                dbSet.Update(original);
                if (isNew)
                {
                    context.Entry(original).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        public static void SaveSubCollection<T>(List<T> toAdd, List<T> toUpdate, List<T> toDelete)
            where T : ProjectModel
        {
            using (var DbContext = DbContextCreator.Create())
            {
                var dbSet = DbContext.Set<T>();
                dbSet.RemoveRange(toDelete);
                toAdd.ForEach(item => DbContext.Entry(item).State = EntityState.Added);
                dbSet.UpdateRange(toUpdate);
                DbContext.SaveChanges();
            }
        }
        
    }
}

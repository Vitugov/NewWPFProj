using EntityToWindow.Core;
using Microsoft.EntityFrameworkCore;

namespace EntityToWindow.DbActions
{
    public static class DbHandler
    {
        public static List<ProjectModel> GetSet(this Type type)
        {
            using (var context = DbContextCreator.Create())
            {
                return context.Set(type).ToList();
            }
        }

        public static T GetDeepSetForObj<T>(this T obj)
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

        public static void DeleteItem<T>(this T item)
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

        public static void SaveItem<T>(this T edit, T original, bool isNew)
            where T : ProjectModel
        {
            ClassOverview.InvokeSaver(typeof(SubCollectionSaver<>), edit, original);

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

        public static List<ProjectModel> GetLinksOnItem<T>(this T item)
            where T : ProjectModel
        {
            var linksList = new List<ProjectModel>();
            foreach (var userClassOverview in Info.GetAllUserClassesOverview())
            {
                foreach (var property in userClassOverview.PropertiesOfUserClass.Where(prop => typeof(T) == prop.PropertyType))
                {
                    List<ProjectModel> resultList;
                    using (var DbContext = DbContextCreator.Create())
                    {
                        resultList = DbContext
                            .ShallowSet(userClassOverview.Type)
                            .ToList()
                            .Where(obj => ((T)property.GetValue(obj)).Id == item.Id)
                            .ToList();
                    }
                    if (resultList.Any())
                    {
                        linksList.AddRange(resultList);
                    }
                }
            }
            return linksList;
        }
    }
}

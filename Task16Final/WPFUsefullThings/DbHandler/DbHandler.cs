using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class DbHandler
    {
        public Type HandeledClass { get; set; }
        public ClassOverview ClassOverview { get; set; }

        public DbHandler(Type type)
        {
            HandeledClass = type;
            ClassOverview = ClassOverview.Dic[HandeledClass.Name];
        }

        public static ObservableCollection<T> GetDeepData<T>(DbContext dbContext)
            where T : ProjectModel
        {
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            var itemCollection = new ObservableCollection<T>();
                IQueryable<T> query = dbContext.Set<T>();
                foreach (var property in classOverview.PropertiesOfCoreClass)
                {
                    query = query.Include(property.Name);
                }
                itemCollection = new ObservableCollection<T>(query);
            return itemCollection;
        }

        public static IQueryable<ProjectModel> GetData(DbContext dbContext, Type type)
        {
            var method = dbContext.GetType().GetMethods().Single(p =>
                p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

            method = method.MakeGenericMethod(type);
            return (IQueryable<ProjectModel>)method.Invoke(dbContext, null) ;
        }

        public static IQueryable<ProjectModel> GetDeepData(DbContext dbContext, Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            var query = GetData(dbContext, type);

            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.PropertyType.Name);
            }
            return query;
        }
    }
}

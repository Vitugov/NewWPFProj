using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public static class DbHandler
    {
        public static ObservableCollection<T> GetDeepData<T>(this DbContext context)
            where T : class, IProjectModel
        {
            var itemCollection = new ObservableCollection<T>();
            var propertiesToInclude = typeof(T).GetPropertiesOfType(typeof(IProjectModel));
            using (context)
            {
                IQueryable<T> query = context.Set<T>();
                foreach (var property in propertiesToInclude)
                {
                    query = query.Include(property.Name);
                }
                itemCollection = new ObservableCollection<T>(query);
            }
            return itemCollection;
        }

        public static IQueryable<IProjectModel> GetData(this DbContext context, Type type)
        {
            var method = typeof(DbContext).GetMethods().Single(p =>
                p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

            method = method.MakeGenericMethod(type);

            return method.Invoke(context, null) as IQueryable<IProjectModel>;
        }

        public static IQueryable<IProjectModel> GetDeepData(this DbContext context, Type type)
        {

            var propertiesToInclude = type.GetPropertiesOfType(typeof(IProjectModel));

            var query = context.GetData(type);

            foreach (var property in propertiesToInclude)
            {
                query = query.Include(property.PropertyType.Name);
            }
            return query;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Sdk.Sfc;
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
        public static ObservableCollection<T> GetDeepData<T>(this DbContext dbContext)
            where T : ProjectModel
        {
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            IQueryable<T> query = dbContext.Set<T>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.Name);
            }
            if (classOverview.HaveCollection)
            {
                query = query.Include(classOverview.CollectionProperty.Name);
                foreach (var property in classOverview.CollectionGenericClassOverview.PropertiesOfCoreClass)
                {
                    //var path = $"{classOverview.CollectionProperty.Name}.SelectMany(e => e.{property.Name})";
                    //query = query.Include(e => (IEnumerable<ProjectModel>)e, classOverview.CollectionProperty.Name)
                    //              .Select(x => EF.Property<ProjectModel>(x, property.Name)));
                }
            }
            using (var context = DbContextCreator.Create())
            {
                return new ObservableCollection<T>(query);
            }
        }

        public static IQueryable<ProjectModel> GetData(this DbContext dbContext, Type type)
        {
            var method = dbContext.GetType().GetMethods().Single(p =>
                p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

            method = method.MakeGenericMethod(type);
            using (var context = DbContextCreator.Create())
            {
                var result = (IQueryable<ProjectModel>)method.Invoke(dbContext, null);
                return result;
            }
        }

        public static IQueryable<ProjectModel> GetDeepData(this DbContext dbContext, Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            var query = GetData(dbContext, type);

            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.Name);
            }
            if (classOverview.HaveCollection)
            {
                query = query.Include(classOverview.CollectionProperty.Name);
                foreach (var property in classOverview.CollectionGenericClassOverview.PropertiesOfCoreClass)
                {
                    //var path = $"d => d.{classOverview.CollectionProperty.Name}.SelectMany(e => e.{property.Name})";
                    //query = query.Include(e => EF.Property<IEnumerable<ProjectModel>>(e, classOverview.CollectionProperty.Name)
                    //    .Select(x => EF.Property<ProjectModel>(x, property.Name)));
                }
            }
            return query;
        }
    }
}

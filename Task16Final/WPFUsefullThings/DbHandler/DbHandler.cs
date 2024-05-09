using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;
using NooberCong.DynamicInclude.Extensions;

namespace WPFUsefullThings
{
    public static class DbHandler
    {
        public static IQueryable<T> ShallowSet<T>(this DbContext dbContext)
            where T : ProjectModel
        {
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            IQueryable<T> query = dbContext.Set<T>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.Name);
            }
            return query;
        }
        public static IQueryable<T> DeepSet<T>(this DbContext dbContext)
            where T : ProjectModel
        {
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            IQueryable<T> query = dbContext.Set<T>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.SuperInclude(property.Name);
            }
            if (classOverview.HaveCollection)
            {
                query = query.SuperInclude(classOverview.CollectionProperty.Name);
                foreach (var property in classOverview.CollectionGenericClassOverview.PropertiesOfCoreClass)
                {
                    query = query.Include($"{classOverview.CollectionProperty.Name}.{property.Name}");
                }
            }
            return query;
        }

        public static IQueryable<ProjectModel> DeepSet(this DbContext dbContext, Type type)
        {
            ProjectModel obj = (ProjectModel)Activator.CreateInstance(type);
            return dbContext.DeepSet(obj);
        }

        public static IQueryable<T> DeepSet<T>(this DbContext dbContext, T obj)
            where T : ProjectModel
        {
            return dbContext.DeepSet<T>();
        }

        public static IQueryable<ProjectModel> Set(this DbContext dbContext, Type type)
        {
            var method = dbContext.GetType().GetMethods().Single(p =>
                            p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

            method = method.MakeGenericMethod(type);

            var result = (IQueryable<ProjectModel>)method.Invoke(dbContext, null);
            return result;
        }
    }
}

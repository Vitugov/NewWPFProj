using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.HadrData;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFUsefullThings
{
    public static class Extensions
    {
        public static IEnumerable<PropertyInfo?> GetPropertiesOfType(this Type typeIn, Type typeOf)
        {
            var Properties = typeIn.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesToInclude = new List<PropertyInfo?>();
            foreach (var property in Properties)
            {
                if (typeOf.IsAssignableFrom(property.PropertyType))
                {
                    propertiesToInclude.Add(property);
                }
            }
            return propertiesToInclude;
        }

        public static bool IsContainingString(this object obj, string str)
        {
            var objectProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (str == null || str == "")
                return true;
            var result = objectProperties.Any(property => property.GetValue(obj).ToString().Contains(str));
            return result;
        }

        public static ObservableCollection<T> GetDeepData<T>(this DbContext context)
            where T : class, IProjectModel
        {
            var itemCollection = new ObservableCollection<T>();
            var propertiesToInclude = GetPropertiesOfType(typeof(T), typeof(IProjectModel));
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

        public static Dictionary<string, ObservableCollection<KeyValuePair<string, IProjectModel>>>
            GetDictionariesOfRelatedProperties(this DbContext context, Type type)
        {
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, IProjectModel>>>();
            var properties = type.GetPropertiesOfType(typeof(IProjectModel));
            foreach (var property in properties)
            {
                using (context)
                {
                    var set = context.GetDeepData(property.PropertyType);
                    var keyValuePairSet = set.Select(obj => new KeyValuePair<string, IProjectModel>(obj.ToViewString() ?? "", obj));
                    var collection = new ObservableCollection<KeyValuePair<string, IProjectModel>>(keyValuePairSet);
                    dic[property.Name] = collection;
                }
            }
            return dic;
        }

        public static IQueryable<IProjectModel> GetDeepData(this DbContext context, Type type)
        {
            ObservableCollection<IProjectModel> itemCollection;
            var propertiesToInclude = GetPropertiesOfType(type, typeof(IProjectModel));
            //using (context)
            //{
                var query = context.GetData(type);

                foreach (var property in propertiesToInclude)
                {
                    query = query.Include(property.Name);
                }
                return query;
            //}
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.HadrData;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFUsefullThings
{
    public static class Extensions
    {
        public static bool IsContainingString(this object obj, string str)
        {
            var objectProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (string.IsNullOrEmpty(str))
                return true;
            var result = objectProperties
                        .Any(property =>
                        {
                            var propertyValue = property.GetValue(obj);
                            return propertyValue != null && propertyValue.ToString().ToLower().Contains(str.ToLower());
                        });

            return result;
        }


        private static MethodInfo _include = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo()
            .GetDeclaredMethods("Include")
            .Single((MethodInfo mi) => mi.GetGenericArguments().Length == 2 && mi
                .GetParameters()
                .Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

        private static MethodInfo _thenIncludeReference = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods()
            .Where(m => m.Name == "ThenInclude")
            .Single(m => m.Name == "ThenInclude" &&
                         m.GetParameters()
                          .Single(p => p.Name == "source")
                          .ParameterType
                          .GetGenericArguments()[1].Name != typeof(ICollection<>).Name);

        private static MethodInfo _thenIncludeCollection = typeof(EntityFrameworkQueryableExtensions)
            .GetMethods()
            .Where(m => m.Name == "ThenInclude")
            .Single(m => m.Name == "ThenInclude" &&
                         m.GetParameters()
                          .Single(p => p.Name == "source")
                          .ParameterType
                          .GetGenericArguments()[1].Name == typeof(ICollection<>).Name);

        public static IQueryable<T> SuperInclude<T>(this IQueryable<T> query, string include)
        {
            return query.SuperInclude(include.Split('.'));
        }

        public static IQueryable<T> SuperInclude<T>(this IQueryable<T> query, params string[] include)
        {
            var currentType = query.ElementType;
            var previousNavWasCollection = false;

            for (int i = 0; i < include.Length; i++)
            {
                var navigationName = include[i];
                var navigationProperty = currentType.GetProperty(navigationName);
                if (navigationProperty == null)
                {
                    throw new ArgumentException($"'{navigationName}' is not a valid property of '{currentType}'");
                }

                MethodInfo includeMethod;
                if (i == 0)
                {
                    includeMethod = _include.MakeGenericMethod([query.ElementType, navigationProperty.PropertyType]);
                }
                else
                {
                    if (previousNavWasCollection)
                    {
                        includeMethod = _thenIncludeCollection.MakeGenericMethod(query.ElementType, currentType, navigationProperty.PropertyType);
                    }
                    else
                    {
                        includeMethod = _thenIncludeReference.MakeGenericMethod(query.ElementType, currentType, navigationProperty.PropertyType);
                    }
                }

                var expressionParameter = Expression.Parameter(currentType);
                var expression = Expression.Lambda(
                    Expression.Property(expressionParameter, navigationName),
                    expressionParameter);

                query = (IQueryable<T>)includeMethod.Invoke(null, new object[] { query, expression });

                if (navigationProperty.PropertyType.GetInterfaces().Any(x => x.Name == typeof(ICollection<>).Name))
                {
                    previousNavWasCollection = true;
                    currentType = navigationProperty.PropertyType.GetGenericArguments().Single();
                }
                else
                {
                    previousNavWasCollection = false;
                    currentType = navigationProperty.PropertyType;
                }
            }

            return query;
        }



    }
}

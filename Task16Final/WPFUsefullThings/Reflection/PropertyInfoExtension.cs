using Castle.Core.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public static class PropertyInfoExtension
    {
        public static IEnumerable<PropertyInfo> IsVisible(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.GetAttribute<InvisibleAttribute>() == null);
        }

        public static IEnumerable<PropertyInfo> NotCollection(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => !property.IsCollection() || property.PropertyType == typeof(string));
        }

        public static IEnumerable<PropertyInfo> IsCollection(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.IsCollection() && property.PropertyType != typeof(string));
        }
        public static bool IsCollection(this PropertyInfo property)
        {
            return property.PropertyType.IsAssignableTo(typeof(IEnumerable)) && property.PropertyType != typeof(string);
        }

        public static IEnumerable<PropertyInfo> IsEasyType(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.IsEasyType());
        }
        public static bool IsEasyType(this PropertyInfo property)
        {
            return (property.PropertyType.IsValueType && !property.PropertyType.IsEnum)
                    || property.PropertyType == typeof(string);
        }

        public static IEnumerable<PropertyInfo> IsUserType(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.PropertyType.IsUserClass());
        }

    }
}

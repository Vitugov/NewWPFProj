﻿using System.Collections;
using System.Reflection;

namespace EntityToWindow.Core
{
    public static class PropertyInfoExtension
    {
        public static IEnumerable<PropertyInfo> IsVisible(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.IsVisible());
        }

        public static bool IsVisible(this PropertyInfo property)
        {
            return property.GetCustomAttribute<InvisibleAttribute>() == null && property.GetGetMethod(false) != null;
        }

        public static IEnumerable<PropertyInfo> NotCollection(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => !property.IsCollection());
        }

        public static IEnumerable<PropertyInfo> IsCollection(this IEnumerable<PropertyInfo> enumerable)
        {
            return enumerable.Where(property => property.IsCollection());
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

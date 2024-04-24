﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WPFUsefullThings.Reflection
{
    public class ClassOverview
    {
        public static Dictionary<string, ClassOverview> Dic { get; set; } = [];
        public static Type CoreInterface { get; set; } = typeof(IProjectModel);

        public string Name { get; set; }
        public string DisplayNameSingular { get; set; }
        public string DisplayNamePlural { get; set; }

        public Type Type { get; set; }
        public PropertyInfo[] Properties { get; set; }

        public Dictionary<string, string> PropertiesDisplayNames { get; set; }
        public PropertyInfo[] CoreInterfaceProperties { get; set; }
        public bool HaveCollection { get; set; }
        public PropertyInfo? CollectionProperty { get; set; }
        public Type? CollectionGenericParameter { get; set; }
        public ClassOverview? CollectionGenericClassOverview { get; set; }

        public ClassOverview(Type type)
        {
            Type = type;
            Name = type.Name;
            var displayAttribute = GetClassDisplayName();
            DisplayNameSingular = displayAttribute.Singular;
            DisplayNamePlural = displayAttribute.Plural;

            if (!CoreInterface.IsAssignableFrom(Type))
                throw new Exception($"Type {Name} does not implement {CoreInterface} interface.");
            Properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertiesDisplayNames = GetPropertiesDisplayNamesDic();

            CoreInterfaceProperties = GetPropertiesOfCoreInterface();
            CollectionProperty = GetIEnumerableProperty();
            HaveCollection = CollectionProperty != null;
            CollectionGenericParameter = GetIEnumerableGeneric();
            if (CollectionGenericParameter != null)
            {
                if (!Dic.ContainsKey(CollectionGenericParameter.Name))
                {
                    new ClassOverview(CollectionGenericParameter);
                }
                CollectionGenericClassOverview = Dic[CollectionGenericParameter.Name];
            }
            Dic.Add(Name, this);
        }

        private PropertyInfo[] GetPropertiesOfCoreInterface()
        {
            var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(property => CoreInterface.IsAssignableFrom(property.PropertyType))
                .ToArray();
        }

        private Dictionary<string, string> GetPropertiesDisplayNamesDic()
        {
            var dic = new Dictionary<string, string>();
            foreach (var property in Properties)
            {
                dic[property.Name] = GetPropertyDisplayName(property);
            }
            return dic;
        }

        private static string GetPropertyDisplayName(PropertyInfo property)
        {
            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            return displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
        }

        private DisplayNamesAttribute GetClassDisplayName()
        {
            var displayNamesAttribute = Type.GetCustomAttribute<DisplayNamesAttribute>();
            if (displayNamesAttribute == null)
            {
                throw new Exception($"Class {Name} doesn't have DisplayNamesAttribute");
            }
            return displayNamesAttribute;
        }

        private PropertyInfo? GetIEnumerableProperty()
        {
            var propertyWithCollection = GetCollectionProperties();

            if (!propertyWithCollection.Any())
            {
                return null;
            }

            return propertyWithCollection.First();
        }

        private Type GetIEnumerableGeneric()
        {
            var propertyType = GetIEnumerableProperty().PropertyType;
            Type? elementType = propertyType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();

            if (elementType == null || !CoreInterface.IsAssignableFrom(elementType))
            {
                throw new Exception($"There is no generic or generic does not implement {CoreInterface}");
            }

            return elementType;
        }

        private IEnumerable<PropertyInfo> GetCollectionProperties()
        {
            var result = Properties
                .Where(property => property.PropertyType.GetInterfaces().Any(i => i == typeof(IEnumerable) && i != typeof(string)));
            if (result.Count() > 1)
                throw new Exception($"Class {Type.FullName} consists more than one collection.");
            return result;
        }
    }
}

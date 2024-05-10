using Castle.Core.Internal;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace WPFUsefullThings
{
    public class ClassOverview
    {
        public string Name { get; set; }
        public string DisplayNameSingular { get; set; }
        public string DisplayNamePlural { get; set; }

        public bool IsSubClass { get; set; }
        public Type Type { get; set; }
        public PropertyInfo[] Properties { get; set; }

        public Dictionary<string, string> PropertiesDisplayNames { get; set; }

        public PropertyInfo[] PropertiesOfUserClass { get; set; }
        public bool HaveCollection { get; set; }
        public bool IsCollectionSubClass { get; set; }
        public bool HaveSubCollection => HaveCollection && IsCollectionSubClass;
        public PropertyInfo? CollectionProperty { get; set; }
        public Type? CollectionGenericParameter { get; set; }
        
        public ClassOverview(Type type)
        {
            Type = type;
            Name = type.Name;
            IsSubClass = GetIsSubClass(type);
            var displayAttribute = GetClassDisplayName();
            DisplayNameSingular = displayAttribute.Singular;
            DisplayNamePlural = displayAttribute.Plural;

            Properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertiesDisplayNames = GetPropertiesDisplayNamesDic();

            PropertiesOfUserClass = GetPropertiesOfCoreClass();
            CollectionProperty = GetIEnumerableProperty();
            HaveCollection = CollectionProperty != null;
            if (HaveCollection)
            {
                CollectionGenericParameter = GetIEnumerableGeneric();
                IsCollectionSubClass = GetIsSubClass(CollectionGenericParameter);
            }
        }

        public ProjectModel CreateObject()
        {
            return (ProjectModel)Activator.CreateInstance(Type);
        }

        
        private PropertyInfo[] GetPropertiesOfCoreClass()
        {
            var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(property => property.PropertyType.IsUserClass())
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

        private bool GetIsSubClass(Type type)
        {
            var subClassAttribute = type.GetCustomAttribute<SubClassAttribute>();
            return subClassAttribute != null ? true : false;
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

            if (elementType == null || !elementType.IsUserClass())
            {
                throw new Exception($"There is no generic or generic does not implement {Info.CoreClass}");
            }

            return elementType;
        }

        private IEnumerable<PropertyInfo> GetCollectionProperties()
        {
            var result = Properties
                .Where(property => property.PropertyType.GetInterfaces().Any(i => i == typeof(IEnumerable) && property.PropertyType != typeof(string)));
            if (result.Count() > 1)
                throw new Exception($"Class {Type.FullName} consists more than one collection.");
            return result;
        }

        public IList GetCollectionFor(object obj)
        {
            if (!HaveSubCollection)
                throw new Exception($"Class {Type.Name} has no SubCollection");
            var collectionProperty = Type.GetProperty(CollectionProperty.Name);
            var collection = (IList)collectionProperty.GetValue(obj);
            return collection;
        }
    }
}

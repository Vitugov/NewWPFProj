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
        public List<PropertyInfo> Properties { get; set; }

        public Dictionary<string, string> PropertiesDisplayNames { get; set; }

        public List<PropertyInfo> PropertiesOfUserClass { get; set; }
        public List<CollectionPropertyOverview> CollectionProperties { get; set; } = [];
        public bool HaveCollection => CollectionProperties.Any();
        public bool HaveSubCollection => HaveCollection ? CollectionProperties[0].IsGenericClassSubClass : false;
        public PropertyInfo? CollectionProperty => HaveCollection ? CollectionProperties[0].Property : null;
        public Type? CollectionGenericParameter => HaveCollection ? CollectionProperties[0].GenericParameter : null;

        public ClassOverview(Type type)
        {
            Type = type;
            Name = type.Name;
            IsSubClass = GetIsSubClass(type);
            var displayAttribute = GetClassDisplayName();
            DisplayNameSingular = displayAttribute.Singular;
            DisplayNamePlural = displayAttribute.Plural;
            Properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            PropertiesDisplayNames = GetPropertiesDisplayNamesDic();
            PropertiesOfUserClass = GetPropertiesOfCoreClass();
            CollectionProperties = GetCollectionProperties()
                .Select(property => new CollectionPropertyOverview(property))
                .ToList();
        }

        public ProjectModel CreateObject()
        {
            return (ProjectModel)Activator.CreateInstance(Type);
        }

        
        private List<PropertyInfo> GetPropertiesOfCoreClass()
        {
            var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(property => property.PropertyType.IsUserClass())
                .ToList();
        }

        private Dictionary<string, string> GetPropertiesDisplayNamesDic()
        {
            var dic = new Dictionary<string, string>();
            Properties
                .ToList()
                .ForEach(property => dic[property.Name] = GetDisplayName(property));
            return dic;
        }

        public static string GetDisplayName(PropertyInfo property)
        {
            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            return displayNameAttribute != null ? displayNameAttribute.DisplayName : property.Name;
        }

        private DisplayNamesAttribute GetClassDisplayName()
        {
            var displayNamesAttribute = Type.GetCustomAttribute<DisplayNamesAttribute>();
            return displayNamesAttribute != null ? displayNamesAttribute :
                new DisplayNamesAttribute(Type.Name, Type.Name);
        }

        private bool GetIsSubClass(Type type)
        {
            var subClassAttribute = type.GetCustomAttribute<SubClassAttribute>();
            return subClassAttribute != null ? true : false;
        }

        private IEnumerable<PropertyInfo> GetCollectionProperties()
        {
            var result = Properties
                .Where(property => property.PropertyType.GetInterfaces()
                    .Any(i => i == typeof(IEnumerable) && property.PropertyType != typeof(string)));
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

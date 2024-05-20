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

        public bool IsSubClass { get; }
        public Type Type { get; }
        public List<PropertyInfo> Properties { get; }

        public Dictionary<string, string> PropertiesDisplayNames { get; }

        public List<PropertyInfo> PropertiesOfUserClass { get; }
        public List<CollectionPropertyOverview> CollectionProperties { get; } = [];
        public List<CollectionPropertyOverview> SubClassCollectionProperties => CollectionProperties
            .Where(property => property.IsGenericClassSubClass)
            .ToList();

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

        public static IList GetCollectionFor(CollectionPropertyOverview collectionPropertyOverview, object obj)
        {
            return (IList)collectionPropertyOverview.Property.GetValue(obj);
        }

        public static void InvokeSaver<T>(Type type, T edit, T original)
        {
            var classOverview = typeof(T).GetClassOverview();
            var collections = classOverview.CollectionProperties;
            foreach (var collection  in collections)
            {
                if (collection.IsGenericClassSubClass)
                {
                    var editCollection = GetCollectionFor(collection, edit);
                    var originalCollection = GetCollectionFor(collection, original);
                    Type genericListType = type.MakeGenericType(collection.GenericParameter);
                    Activator.CreateInstance(genericListType, editCollection, originalCollection);
                }
            }
        }
    }
}

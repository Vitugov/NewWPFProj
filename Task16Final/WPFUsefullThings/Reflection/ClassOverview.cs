using Microsoft.SqlServer.Management.HadrModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace WPFUsefullThings
{
    public class ClassOverview
    {
        public static Dictionary<string, ClassOverview> Dic { get; set; } = [];
        public static Type CoreClass { get; set; } = typeof(ProjectModel);
        public static Type[] AllDerivedClasses { get; set; }


        public string Name { get; set; }
        public string DisplayNameSingular { get; set; }
        public string DisplayNamePlural { get; set; }

        public bool IsSubClass { get; set; }
        public Type Type { get; set; }
        public PropertyInfo[] Properties { get; set; }

        public Dictionary<string, string> PropertiesDisplayNames { get; set; }

        public PropertyInfo[] PropertiesOfCoreClass { get; set; }
        public bool HaveCollection { get; set; }
        public bool IsCollectionSubClass { get; set; }
        public PropertyInfo? CollectionProperty { get; set; }
        public Type? CollectionGenericParameter { get; set; }
        public ClassOverview? CollectionGenericClassOverview { get; set; }

        static ClassOverview()
        {
            AllDerivedClasses = GetAllDerivedClasses();
            if (AllDerivedClasses.Any())
            {
                foreach (var derivedClass in AllDerivedClasses)
                {
                    if (!Dic.ContainsKey(derivedClass.Name))
                    {
                        _ = new ClassOverview(derivedClass);
                    }
                }
            }
        }
        
        public ClassOverview(Type type)
        {
            Type = type;
            Name = type.Name;
            IsSubClass = GetIsSubClass(type);
            var displayAttribute = GetClassDisplayName();
            DisplayNameSingular = displayAttribute.Singular;
            DisplayNamePlural = displayAttribute.Plural;

            if (!CoreClass.IsAssignableFrom(Type))
                throw new Exception($"Type {Name} does not implement {CoreClass}.");
            Properties = Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertiesDisplayNames = GetPropertiesDisplayNamesDic();

            PropertiesOfCoreClass = GetPropertiesOfCoreClass();
            CollectionProperty = GetIEnumerableProperty();
            HaveCollection = CollectionProperty != null;
            if (HaveCollection)
            {
                CollectionGenericParameter = GetIEnumerableGeneric();
                IsCollectionSubClass = GetIsSubClass(CollectionGenericParameter);

                if (!Dic.ContainsKey(CollectionGenericParameter.Name))
                {
                    new ClassOverview(CollectionGenericParameter);
                }
                CollectionGenericClassOverview = Dic[CollectionGenericParameter.Name];
            }
            if (!Dic.ContainsKey(Name))
            {
                Dic[Name] = this;
            }
        }

        private static Type[] GetAllDerivedClasses()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Находим все типы, которые являются наследниками ProjectModel во всех сборках
            var allDerivedTypes = assemblies.SelectMany(assembly =>
            {
                try
                {
                    return assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(CoreClass));
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // В случае ошибок загрузки типа, возвращаем только успешно загруженные типы
                    return ex.Types.Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(CoreClass));
                }
            }).ToArray();

            return allDerivedTypes;
        }

        public ProjectModel CreateObject()
        {
            return (ProjectModel)Activator.CreateInstance(Type);
        }

        
        private PropertyInfo[] GetPropertiesOfCoreClass()
        {
            var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties
                .Where(property => CoreClass.IsAssignableFrom(property.PropertyType))
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
            if (subClassAttribute == null)
            {
                return false;
            }
            return subClassAttribute.IsSubClass;
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

            if (elementType == null || !CoreClass.IsAssignableFrom(elementType))
            {
                throw new Exception($"There is no generic or generic does not implement {CoreClass}");
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

        public bool HaveSubCollection => HaveCollection && IsCollectionSubClass;

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

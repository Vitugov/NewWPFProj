using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public static class Info
    {
        private static Dictionary<string, ClassOverview> ClassOverviewDic { get; set; } = [];
        public static Type CoreClass { get; } = typeof(ProjectModel);
        private static Type[] AllUserClasses { get; set; }
        private static List<KeyValuePair<string, Type>> MainWindowClasses { get; set; } = [];

        static Info()
        {
            AllUserClasses = GetAllUserClasses();
            MainWindowClasses = GetTypesForMainWindow();
            FillDictionary();
        }

        private static Type[] GetAllUserClasses()
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
                    return ex.Types
                        .Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(CoreClass));
                }
            }).ToArray();

            if (allDerivedTypes == null || !allDerivedTypes.Any())
            {
                throw new Exception($"There is no classes dervived from {CoreClass.Name}." +
                    $"Only classes derived from {CoreClass.Name} could be showen");
            }

            return allDerivedTypes;
        }

        private static List<KeyValuePair<string, Type>> GetTypesForMainWindow()
        {
            return AllUserClasses
                .Where(type => type.GetAttribute<SubClassAttribute>() == null)
                .Select(type => new KeyValuePair<string, Type>(type.GetAttribute<DisplayNamesAttribute>().Plural, type))
                .OrderBy(pair => pair.Key)
                .ToList();
        }

        private static void FillDictionary()
        {
            foreach (var derivedClass in AllUserClasses)
            {
                if (!ClassOverviewDic.ContainsKey(derivedClass.Name))
                {
                    var classOverview = new ClassOverview(derivedClass);
                    ClassOverviewDic[classOverview.Name] = classOverview;
                }
            }
        }

        private static ClassOverview GetClassOverview(string name)
        {
            ClassOverview? classOverView;
            var IsSuccess = ClassOverviewDic.TryGetValue(name, out classOverView);
            if (!IsSuccess || classOverView == null) 
            {
                throw new Exception($"Dictionary doesn't contain class {name}.");
            }
            return classOverView;
        }

        public static ClassOverview GetClassOverview(this Type type)
        {
            return GetClassOverview(type.Name);
        }

        public static bool IsUserClass(this Type type)
        {
            return AllUserClasses.Contains(type);
        }

        public static List<KeyValuePair<string, Type>> GetMainWindowClasses()
        {
            return MainWindowClasses;
        }

        public static List<ClassOverview> GetAllUserClassesOverview()
        {
            return ClassOverviewDic.Values.ToList();
        }
    }
}

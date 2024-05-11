using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Reflection;


namespace WPFUsefullThings
{
    public static class DictionaryBuilder
    {
        public static Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>
            GetDictionariesOfRelatedProperties(this Type type)
        {
            var classOverview = type.GetClassOverview();
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>();
            foreach (var property in classOverview.PropertiesOfUserClass)
            {
                dic[property.PropertyType.Name] = GetCollectionForProperty(property);
            }
            return dic;
        }

        public static ObservableCollection<KeyValuePair<string, ProjectModel>> GetCollectionForProperty(PropertyInfo property)
        {
            using (var context = DbContextCreator.Create())
            {
                var set = context.Set(property.PropertyType);

                var keyValuePairSet = set
                    .Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj))
                    .ToList()
                    .OrderBy(pair => pair.Key);
                var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
                return collection;
            }
        }
    }
}

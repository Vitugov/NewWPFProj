using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;


namespace WPFUsefullThings
{
    public static class DictionaryBuilder
    {
        public static Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>
            GetDictionariesOfRelatedProperties(this DbContext cont, Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                using (var context = DbContextCreator.Create())
                {
                    var set = context.Set(property.PropertyType);

                    var keyValuePairSet = set
                        .Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj))
                        .ToList()
                        .OrderBy(pair => pair.Key);
                    var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
                    dic[property.PropertyType.Name] = collection;
                }
            }
            return dic;
        }
    }
}

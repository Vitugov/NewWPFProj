using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class ClassComboDictionary
    {
        private Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> dic = [];
        public ObservableCollection<KeyValuePair<string, ProjectModel>> this[string type] { get => dic[type]; }

        public ClassComboDictionary(Type type)
        {
            type
                .GetClassOverview()
                .PropertiesOfUserClass
                .ForEach(property => dic[property.Name] = GetCollectionForProperty(property));
        }

        private static ObservableCollection<KeyValuePair<string, ProjectModel>> GetCollectionForProperty(PropertyInfo property)
        {
            var keyValuePairSet = DbHandler.GetSet(property.PropertyType)
                .Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj))
                .OrderBy(pair => pair.Key)
                .ToList();
            var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
            return collection;
            //using (var context = DbContextCreator.Create())
            //{
            //    var set = context.Set(property.PropertyType);

            //    var keyValuePairSet = set
            //        .Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj))
            //        .ToList()
            //        .OrderBy(pair => pair.Key);
            //    var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
            //    return collection;
            //}
        }
    }
}

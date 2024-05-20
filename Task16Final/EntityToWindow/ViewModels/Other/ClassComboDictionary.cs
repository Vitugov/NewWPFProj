using EntityToWindow.Core;
using EntityToWindow.DbActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityToWindow.ViewModels
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
            var keyValuePairSet = property.PropertyType.GetSet()
                .Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj))
                .OrderBy(pair => pair.Key)
                .ToList();
            var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
            return collection;
        }
    }
}

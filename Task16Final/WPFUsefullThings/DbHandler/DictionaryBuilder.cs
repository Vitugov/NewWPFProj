using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public static class DictionaryBuilder
    {
        public static Dictionary<string, ObservableCollection<KeyValuePair<string, IProjectModel>>>
            GetDictionariesOfRelatedProperties(this DbContext context, Type type)
        {
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, IProjectModel>>>();
            var properties = type.GetPropertiesOfType(typeof(IProjectModel));
            foreach (var property in properties)
            {
                using (context)
                {
                    var set = context.GetDeepData(property.PropertyType);
                    var keyValuePairSet = set.Select(obj => new KeyValuePair<string, IProjectModel>(obj.ToViewString() ?? "", obj));
                    var collection = new ObservableCollection<KeyValuePair<string, IProjectModel>>(keyValuePairSet);
                    dic[property.Name] = collection;
                }
            }
            return dic;
        }
    }
}

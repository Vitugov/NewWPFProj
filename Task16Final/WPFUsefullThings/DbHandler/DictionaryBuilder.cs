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
        public static Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>
            GetDictionariesOfRelatedProperties(this DbContext context, Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                using (context)
                {
                    var set = DbHandler.GetDeepData(context, property.PropertyType);
                    var keyValuePairSet = set.Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToString(), obj));
                    var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
                    dic[property.Name] = collection;
                }
            }
            return dic;
        }
    }
}

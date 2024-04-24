using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.HadrData;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFUsefullThings
{
    public static class Extensions
    {
        public static bool IsContainingString(this object obj, string str)
        {
            var objectProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (str == null || str == "")
                return true;
            var result = objectProperties.Any(property => property.GetValue(obj).ToString().Contains(str));
            return result;
        }

        public static Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>
            GetDictionariesOfRelatedProperties(this DbContext context, Type type)
        {
            var dic = new Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>>();
            var properties = type.GetPropertiesOfType(typeof(ProjectModel));
            foreach (var property in properties)
            {
                using (context)
                {
                    var set = context.GetDeepData(property.PropertyType);
                    var keyValuePairSet = set.Select(obj => new KeyValuePair<string, ProjectModel>(obj.ToViewString() ?? "", obj));
                    var collection = new ObservableCollection<KeyValuePair<string, ProjectModel>>(keyValuePairSet);
                    dic[property.Name] = collection;
                }
            }
            return dic;
        }


    }
}

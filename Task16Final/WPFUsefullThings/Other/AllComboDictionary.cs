using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class AllComboDictionary
    {
        private Dictionary<string, ClassComboDictionary> dic = [];
        //public ObservableCollection<KeyValuePair<string, ProjectModel>> this[string parameter]
        //{
        //    get
        //    {
        //        try
        //        {
        //            var p = parameter.Split(' ');
        //            return dic[p[0]][p[1]];
        //        }
        //        catch { throw new InvalidOperationException($"Unable to find {parameter} in AllComboDictionary"); }
        //    }
        //}
        public ClassComboDictionary this[string parameter]
        {
            get
            {
                try
                {

                    return dic[parameter];
                }
                catch { throw new InvalidOperationException($"Unable to find {parameter} in AllComboDictionary"); }
            }
        }
        //public ObservableCollection<KeyValuePair<string, ProjectModel>> this[Type type1, string type2] { get => dic[type1][type2]; }
        public AllComboDictionary(Type userType)
        {
             userType.GetClassOverview().CollectionProperties
                .Select(property => property.GenericParameter)
                .Where(type => type.IsUserClass())
                .Append(userType)
                .Distinct()
                .ToList()
                .ForEach(type => dic[type.Name] = new ClassComboDictionary(type));
        }

        public ObservableCollection<KeyValuePair<string, ProjectModel>> Get(string type1, string type2) => dic[type1][type2];
    }
}

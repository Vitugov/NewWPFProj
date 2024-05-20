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
        public ClassComboDictionary this[string userTypeName]
        {
            get
            {
                try { return dic[userTypeName]; }
                catch { throw new InvalidOperationException($"Unable to find {userTypeName} in AllComboDictionary"); }
            }
        }

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
    }
}

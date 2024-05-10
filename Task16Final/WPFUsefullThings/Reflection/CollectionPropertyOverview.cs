using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class CollectionPropertyOverview
    {
        public PropertyInfo Property { get; set; }
        public Type GenericParameter { get; set; }

        public bool IsGenericParameterUserClass => GenericParameter.IsUserClass();
        public bool IsGenericClassSubClass => GenericParameter.IsUserClass() ? GenericParameter.GetClassOverview().IsSubClass : false;
        public ClassOverview? GenericClassOverview => GenericParameter.IsUserClass() ? GenericParameter.GetClassOverview() : null;

        public CollectionPropertyOverview(PropertyInfo property)
        {
            Property = property;
            GenericParameter = GetGeneric(property);
        }

        private Type GetGeneric(PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            Type elementType = propertyType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments()[0])
                .First();
            return elementType;
        }
    }
}

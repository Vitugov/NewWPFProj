using System.Reflection;

namespace EntityToWindow.Core
{
    public static class Extensions
    {
        public static bool IsContainingString(this object obj, string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            var objectProperties = obj.GetType().GetClassOverview().Properties;
            var result = objectProperties
                        .Any(property =>
                        {
                            var propertyValue = property.GetValue(obj);
                            return propertyValue != null && propertyValue.ToString().ToLower().Contains(str.ToLower());
                        });

            return result;
        }


    }
}

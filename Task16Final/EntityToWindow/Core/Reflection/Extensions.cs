using System.Reflection;

namespace EntityToWindow.Core
{
    public static class Extensions
    {
        public static bool IsContainingString(this object obj, string str)
        {
            var objectProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (string.IsNullOrEmpty(str))
                return true;
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

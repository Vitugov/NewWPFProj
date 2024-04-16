using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class DynamicIsValid : DynamicObject
    {
        private ExpandoObject _errors;

        public DynamicIsValid(ExpandoObject errors)
        {
            _errors = errors;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var errorsDict = _errors as IDictionary<string, object>;
            if (errorsDict.ContainsKey(binder.Name))
            {
                var errors = errorsDict[binder.Name] as List<ValidationResult>;
                result = errors != null && !errors.Any();
                return true;
            }

            result = false;
            return false;
        }
    }

    public class ExpandoFactory
    {
        public static ExpandoObject CreateExpando(Type typeToValidate, object initObject)
        {
            var expando = new ExpandoObject();
            var expandoDict = expando as IDictionary<string, object>;

            var properties = typeToValidate.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                expandoDict.Add(property.Name, initObject);
            }

            return expando;
        }
    }
}

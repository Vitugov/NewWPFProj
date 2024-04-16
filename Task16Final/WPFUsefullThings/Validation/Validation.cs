using System;
using System.Dynamic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WPFUsefullThings
{
    public class Validation<T> where T : INotifyPropertyChanged
    {
        private readonly T _objectToValidate;
        public DynamicIsValid IsValid { get; private set; }
        public ExpandoObject Errors { get; private set; }

        private IDictionary<string, object> ErrorsAsDic => Errors as IDictionary<string, object>;

        private PropertyInfo[] _objectToValidateProperies;

        public Validation(T objectToValidate)
        {
            _objectToValidate = objectToValidate;
            _objectToValidate.PropertyChanged += _objectToValidate_PropertyChanged;
            _objectToValidateProperies = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Errors = ExpandoFactory.CreateExpando(typeof(T), Enumerable.Empty<ValidationResult>());
            IsValid = new DynamicIsValid(Errors);
        }

        public IEnumerable<ValidationResult> ValidateProperty(string propertyName)
        {
            var context = new ValidationContext(_objectToValidate)
            {
                MemberName = propertyName
            };
            var Results = new List<ValidationResult>();
            Validator.TryValidateProperty(
                typeof(T).GetProperty(propertyName).GetValue(_objectToValidate, null),
                context,
                Results);
            return Results;
        }

        public bool Validate()
        {
            bool isValid = true;

            foreach (var property in _objectToValidateProperies)
            {
                var results = ValidateProperty(property.Name).ToList();

                ErrorsAsDic[property.Name] = results;

                if (results.Any())
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        public void _objectToValidate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (sender.Equals(_objectToValidate))
            //{
                ValidateProperty(e.PropertyName);
            //}
        }
    }
}

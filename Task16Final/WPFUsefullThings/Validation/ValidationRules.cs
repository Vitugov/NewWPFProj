using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUsefullThings
{
    public static class ValidationRules
    {
        public static bool ValidateEmail(this string email) => email.Contains("@") && email.Contains(".") && email.Length > 4;
        public static bool ValidateMoreThanZero(this int num) => num > 0;
        public static bool ValidateNotEmptyString(this string str) => str.Length > 0;
        public static void ShowErrorMessage() => MessageBox.Show("Некоторые поля имеют недопустимое значение. Поля имеющие недопустимые значения помечены знаком 'х' справа.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        public static bool IsValid(this IEnumerable<bool> validationArray) => !validationArray.Any(e => e == false);

    }
}

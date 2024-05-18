using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace WPFUsefullThings
{
    public class TextBoxFactory : IFrameworkElementFactory
    {
        public int Priority => 0;
        public bool CanHandle(PropertyInfo property) => property.IsEasyType();

        public FrameworkElement Create(PropertyInfo property)
        {
            TextBox textBox = new TextBox
            {
                Width = 200,
                Margin = new Thickness(5)

            };
            if (property.Name == "Id" || property.Name == "DisplayName")
            {
                textBox.IsReadOnly = true;
            }
            Binding binding = new Binding("Item.Edit." + property.Name);
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            if (property.PropertyType == typeof(DateTime))
            {
                binding.StringFormat = "dd.MM.yyyy HH:mm:ss";
            }
            textBox.SetBinding(TextBox.TextProperty, binding);
            return textBox;
        }
    }
}

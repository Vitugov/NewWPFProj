using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WPFUsefullThings
{
    public class ComboBoxFactory : IFrameworkElementFactory
    {
        public int Priority => 10;
        public bool CanHandle(PropertyInfo property) => property.PropertyType.IsUserClass();

        public FrameworkElement Create(PropertyInfo property)
        {
            var propertyName = property.Name;
            ComboBox comboBox = new ComboBox
            {
                Width = 200,
                DisplayMemberPath = "Key",
                SelectedValuePath = "Value",
                Margin = new Thickness(5),
                IsSynchronizedWithCurrentItem = true,
            };

            Binding selectedValueBinding = new Binding("Item.Edit." + propertyName)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            comboBox.SetBinding(ComboBox.SelectedValueProperty, selectedValueBinding);

            Binding itemsSourceBinding = new Binding($"Dic[{propertyName}]");
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);

            return comboBox;
        }


    }
}

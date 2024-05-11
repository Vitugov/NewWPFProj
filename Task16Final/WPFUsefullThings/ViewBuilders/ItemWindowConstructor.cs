using Castle.Core.Internal;
using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WPFUsefullThings
{
    public static class ItemWindowConstructor
    {
        public static StackPanel CreateStackPanel(Type type)
        {
            StackPanel mainPanel = new StackPanel { Orientation = Orientation.Vertical };
            var classOverview = type.GetClassOverview();

            foreach (var property in classOverview.Properties.IsVisible().NotCollection())
            {
                StackPanel horizontalPanel = CreateHorizontalStackPanel(property);
                mainPanel.Children.Add(horizontalPanel);
            }
            return mainPanel;
        }

        public static ComboBox CreateComboBox(string propertyName)
        {
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

        public static TextBlock CreateTextBlock(string text)
        {
            return new TextBlock
            {
                Width = 100,
                Text = text + ": ",
                Margin = new Thickness(5)
            };
        }

        public static TextBox CreateTextBox(PropertyInfo property)
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

        public static StackPanel CreateHorizontalStackPanel(PropertyInfo property)
        {
            StackPanel horizontalPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var text = ClassOverview.GetDisplayName(property);
            var textBlock = CreateTextBlock(text);

            if (property.IsEasyType())
            {
                var textBox = CreateTextBox(property);
                horizontalPanel.Children.Add(textBlock);
                horizontalPanel.Children.Add(textBox);
            }

            if (property.PropertyType.IsUserClass())
            {
                var comboBox = CreateComboBox(property.Name);
                horizontalPanel.Children.Add(textBlock);
                horizontalPanel.Children.Add(comboBox);
            }

            return horizontalPanel;
        }
    }
}

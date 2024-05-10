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
    public static class WindowConstructor
    {
        public static StackPanel InitializeItemWindow(Type type)
        {
            // Основной вертикальный StackPanel для всех пар TextBlock и TextBox
            StackPanel mainPanel = new StackPanel { Orientation = Orientation.Vertical };
            var classOverview = ClassOverview.Dic[type.Name];
            // Перебор всех свойств определенного типа (например, string)
            foreach (PropertyInfo property in type.GetProperties().Where(property => property.GetAttribute<InvisibleAttribute>() == null))
            {
                // Создание горизонтального StackPanel для каждой пары
                StackPanel horizontalPanel = new StackPanel { Orientation = Orientation.Horizontal };

                string? viewName;
                classOverview.PropertiesDisplayNames.TryGetValue(property.Name, out viewName);
                viewName = viewName ?? property.Name;
                // Создание TextBlock для имени свойства
                TextBlock textBlock = new TextBlock
                {
                    Width = 100,
                    Text = viewName + ": ",
                    Margin = new Thickness(5) // небольшой отступ
                };

                if ((property.PropertyType.IsValueType && !property.PropertyType.IsEnum && !typeof(IEnumerable).IsAssignableFrom(type))
                    || property.PropertyType == typeof(string)) // проверка на тип свойства
                {
                    // Создание TextBox для значения свойства
                    TextBox textBox = new TextBox
                    {
                        Width = 200, // фиксированная ширина
                        Margin = new Thickness(5) // небольшой отступ

                    };
                    if (property.Name == "Id")
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

                    // Добавление TextBlock и TextBox в горизонтальный StackPanel
                    horizontalPanel.Children.Add(textBlock);
                    horizontalPanel.Children.Add(textBox);
                }

                if (typeof(ProjectModel).IsAssignableFrom(property.PropertyType))
                {
                    var comboBox = CreateComboBox(property.Name);
                    horizontalPanel.Children.Add(textBlock);
                    horizontalPanel.Children.Add(comboBox);
                }

                // Добавление горизонтального StackPanel в основной вертикальный StackPanel
                mainPanel.Children.Add(horizontalPanel);
            }
            return mainPanel;
        }

        public static DataGrid InitializeCollectionDataGrid(Type type)
        {
            DataGrid dataGrid = new DataGrid();

            Binding itemsSourceBinding = new Binding("ItemCollectionView")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, itemsSourceBinding);

            Binding selectedItemBinding = new Binding("SelectedItem")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            dataGrid.SetBinding(DataGrid.SelectedItemProperty, selectedItemBinding);
            var columnHeaderBehaviour = new ColumnHeaderBehaviour();
            Interaction.GetBehaviors(dataGrid).Add(columnHeaderBehaviour);

            return dataGrid;
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
    }
}

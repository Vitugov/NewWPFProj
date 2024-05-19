using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFUsefullThings
{
    public class DataGridFactory : IFrameworkElementFactory
    {
        public int Priority => 11;

        public bool CanHandle(PropertyInfo property)
        {
            return property.IsVisible() && property.IsCollection();
        }

        public FrameworkElement Create(PropertyInfo property)
        {
            var dataGrid = new DataGrid
            {
                IsReadOnly = false,
            };

            Binding binding = new Binding("EditableItem." + property.Name);
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);
            dataGrid.Tag = property.PropertyType.GetGenericArguments().First();
            dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            return dataGrid;
        }
        public StackPanel CreatePanel(PropertyInfo property)
        {
            return IFrameworkElementFactory.CreatePanelWithOrientation(property, Orientation.Vertical);
        }

        private static void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (typeof(ProjectModel).IsAssignableFrom(e.PropertyType))
            {
                var comboBoxColumn = new DataGridComboBoxColumn
                {
                    Header = e.PropertyName,
                    DisplayMemberPath = "Key",
                    SelectedValuePath = "Value",
                    ItemsSource = ((IItemViewModel)dataGrid.DataContext).ComboDic[((Type)dataGrid.Tag).Name][e.PropertyName],
                    SelectedValueBinding = new Binding(e.PropertyType.Name)
                };
                e.Column = comboBoxColumn;
            }

            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                
                bool isInvisible = descriptor.Attributes[typeof(InvisibleAttribute)] != null;
                bool isCollection = typeof(IEnumerable).IsAssignableFrom(descriptor.PropertyType)
                    && descriptor.PropertyType != typeof(string);
                e.Column.Visibility = !isInvisible && !isCollection ? Visibility.Visible : Visibility.Collapsed;
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUsefullThings.ViewModels;

namespace WPFUsefullThings.View
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        public Type Type { get; set; }

        public ItemWindow(Type type, object context)
        {
            InitializeComponent();
            DataContext = context;
            var stackPanel = WindowConstructor.InitializeItemWindow(type);
            baseGrid.Children.Add(stackPanel);

        }

        private DataGrid? BuildDataGrid(Type type)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (typeof(ICollection).IsAssignableFrom(property.PropertyType))
                {
                    var dataGrid = new DataGrid
                    {
                        IsReadOnly = false,

                    };

                    Binding binding = new Binding("Item.Edit." + property.Name);
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    dataGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);
                    dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                    return dataGrid;
                }

            }
            return null;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            Type dataType = propertyDescriptor.PropertyType;

            if (typeof(IProjectData).IsAssignableFrom(dataType))
            {
                e.Column = new DataGridComboBoxColumn
                {
                    Header = propertyDescriptor.Name,
                    SelectedValueBinding = new Binding("Item.Edit." + propertyDescriptor.Name)
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay,
                    },
                    DisplayMemberPath = "Key",
                    SelectedValuePath = "Value",

                    ItemsSource = ((ItemViewModel)DataContext).Dic[{propertyDescriptor.Name}]) // Метод для получения данных для ComboBox
                };
            }
        }

    }
}

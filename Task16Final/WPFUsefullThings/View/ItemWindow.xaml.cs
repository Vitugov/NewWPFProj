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
using WPFUsefullThings;

namespace WPFUsefullThings
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
            Type = type;
            var stackPanel = WindowConstructor.InitializeItemWindow(type);
            baseGrid.Children.Add(stackPanel);
            var classOverview = ClassOverview.Dic[type.Name];
            var dataGrid = BuildDataGrid(Type);
            stackPanel.Children.Add(dataGrid);

        }

        private DataGrid? BuildDataGrid(Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            if (classOverview.HaveCollection)
            {
                var dataGrid = new DataGrid
                {
                    IsReadOnly = false,
                };

                Binding binding = new Binding("Item.Edit." + classOverview.CollectionProperty.Name);
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                dataGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);
                dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
                return dataGrid;
            }


            return null;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {


            if (typeof(ProjectModel).IsAssignableFrom(e.PropertyType))
            {
                var comboBoxColumn = new DataGridComboBoxColumn
                {
                    Header = e.PropertyName,
                    DisplayMemberPath = "Key",
                    SelectedValuePath = "Value",
                };

                var elementStyle = new Style(typeof(ComboBox));
                elementStyle.Setters.Add(new Setter(ComboBox.DataContextProperty, DataContext));
                elementStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding($"SubCollectionDic[{e.PropertyType.Name}]")));

                var editingElementStyle = new Style(typeof(ComboBox));
                editingElementStyle.Setters.Add(new Setter(ComboBox.DataContextProperty, DataContext));
                editingElementStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding($"SubCollectionDic[{e.PropertyType.Name}]")));

                comboBoxColumn.ElementStyle = elementStyle;
                comboBoxColumn.EditingElementStyle = editingElementStyle;

                e.Column = comboBoxColumn;
            }
        }
    }
    
}

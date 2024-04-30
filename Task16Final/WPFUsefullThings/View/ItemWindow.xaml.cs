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
using WPFUsefullThings.ViewModels;

namespace WPFUsefullThings
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        public Type Type { get; set; }

        public ItemWindow(Type type, IItemViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            Type = type;
            var stackPanel = WindowConstructor.InitializeItemWindow(type);
            baseGrid.Children.Add(stackPanel);
            var classOverview = ClassOverview.Dic[type.Name];
            var dataGrid = BuildDataGrid(Type);
            stackPanel.Children.Add(dataGrid);
            var buttonStackPanel = BuildButtonStackPanel();
            stackPanel.Children.Add(buttonStackPanel);
        }

        private StackPanel BuildButtonStackPanel()
        {
            var viewModel = (IItemViewModel)DataContext;
            var saveButton = new Button
            {
                Content = "Сохранить",
                Command = viewModel.SaveCommand,
                CommandParameter = this
            };
            var cancelButton = new Button
            {
                Content = "Отмена",
                Command = viewModel.CancelCommand,
                CommandParameter = this
            };
            StackPanel buttonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                FlowDirection = FlowDirection.RightToLeft
            };
            buttonStackPanel.Children.Add(saveButton);
            buttonStackPanel.Children.Add(cancelButton);
            return buttonStackPanel;
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
                    ItemsSource = ((IItemViewModel)DataContext).SubCollectionDic[$"{e.PropertyType.Name}"],
                    SelectedValueBinding = new Binding(e.PropertyType.Name)
                };

                e.Column = comboBoxColumn;
            }
        }
    }
    
}

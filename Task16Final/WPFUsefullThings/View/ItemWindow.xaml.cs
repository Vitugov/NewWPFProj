using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


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
            var stackPanel = ItemWindowConstructor.InitializeItemWindow(type);
            baseStackPanel.Children.Add(stackPanel);
            var classOverview = type.GetClassOverview();
            if (classOverview.HaveCollection)
            {
                var dataGrid = BuildDataGrid(Type);
                stackPanel.Children.Add(dataGrid);
            }
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
                CommandParameter = this,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
            };
            var cancelButton = new Button
            {
                Content = "Отмена",
                Command = viewModel.CancelCommand,
                CommandParameter = this,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
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
            var classOverview = type.GetClassOverview();
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

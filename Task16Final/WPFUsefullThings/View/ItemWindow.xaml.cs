using Microsoft.SqlServer.Management.HadrData;
using System.Collections;
using System.Collections.ObjectModel;
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
            var stackPanel = ItemWindowConstructor.CreateStackPanel(type);
            baseStackPanel.Children.Add(stackPanel);
            //var classOverview = type.GetClassOverview();
            //if (classOverview.HaveCollection)
            //{
            //    var dataGrid = FrameworkElementFactory.CreatePanel(classOverview.CollectionProperty);//ItemWindowConstructor.BuildDataGrid(Type);
            //    stackPanel.Children.Add(dataGrid);
            //}
            var buttonStackPanel = ItemWindowConstructor.BuildButtonStackPanel(this);
            stackPanel.Children.Add(buttonStackPanel);
        }
    }
}

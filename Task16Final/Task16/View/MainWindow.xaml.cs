using System.Windows;
using Task16.Model;
using Task16.Other;
using WPFUsefullThings;


namespace Task16.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Initialization.Instance.Init();
            InitializeComponent();
            DataContext = new MainViewModel(typeof(SqliteContext));
            baseGrid.Children.Add(MainWindowConstructor.BuildButtonsStackPanel());
        }
    }
}

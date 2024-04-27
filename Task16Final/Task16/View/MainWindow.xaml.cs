using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICommand OpenClientCollectionWindow {  get; set; }
        public ICommand OpenOrderCollectionWindow { get; set; }

        public MainWindow()
        {
            OpenClientCollectionWindow = new RelayCommand(obj => Execute_OpenClientCollectionWindow());
            OpenOrderCollectionWindow = new RelayCommand(obj => Execute_OpenOrderCollectionWindow());
            DbContextCreator.SetDbContextType(typeof(SqliteContext));
            Initialization.Instance.Init();
            InitializeComponent();
            DataContext = this;
        }

        private void Execute_OpenClientCollectionWindow()
        {
            var viewModel = new CollectionViewModel<Client>(typeof(SqliteContext));
            var view = new CollectionWindow(viewModel);
            view.ShowDialog();
        }

        private void Execute_OpenOrderCollectionWindow()
        {
            var viewModel = new CollectionViewModel<Order>(typeof(SqliteContext));
            var view = new CollectionWindow(viewModel);
            view.ShowDialog();
        }
    }
}

using EntityToWindow.ViewModels;
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

namespace EntityToWindow.WPF
{
    public partial class MainProjectWindow : Window
    {
        public MainProjectWindow(Type dbContextType, Window initWindow)
        {
            initWindow.Close();
            InitializeComponent();
            DataContext =  new MainViewModel(dbContextType);
            baseGrid.Children.Add(MainWindowConstructor.BuildButtonsStackPanel());
        }
    }
}

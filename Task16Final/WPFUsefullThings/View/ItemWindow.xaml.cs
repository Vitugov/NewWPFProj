using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFUsefullThings.ViewModels;

namespace WPFUsefullThings.View
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow<T> : Window
        where T : class, IUpdateable<T>, INotifyPropertyChanged, new()
    {
        public ItemWindow(Type type)
        {
            InitializeComponent();
        }

        public void Construct()
        {
            DataContext as ItemViewModel<T>
        }
    }
}

using System.Windows;


namespace WPFUsefullThings
{
    /// <summary>
    /// Логика взаимодействия для CollectionWindow.xaml
    /// </summary>
    public partial class CollectionWindow : Window
    {
        public CollectionWindow(object context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}

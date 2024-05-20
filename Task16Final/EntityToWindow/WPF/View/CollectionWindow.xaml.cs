using System.Windows;


namespace EntityToWindow.WPF
{
    public partial class CollectionWindow : Window
    {
        public CollectionWindow(object context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}

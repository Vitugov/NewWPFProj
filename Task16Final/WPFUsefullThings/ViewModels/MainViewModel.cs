using System.Windows.Input;

namespace WPFUsefullThings
{
    public class MainViewModel
    {
        public List<KeyValuePair<string, Type>> TypesToView {  get; set; }
        public List<KeyValuePair<string, ICommand>> OpenCollectionWindowCommand { get; set; } = [];

        public MainViewModel(Type dbContextType)
        {
            DbContextCreator.SetDbContextType(dbContextType);
            TypesToView = Info.GetMainWindowClasses();
            foreach (var pair in TypesToView)
            {
                var resultCommand = new RelayCommand(obj => Execute_OpenCollectionWindow(pair.Value));
                var resultpair = new KeyValuePair<string, ICommand>(pair.Key, resultCommand);
                OpenCollectionWindowCommand.Add(resultpair);
            }
        }

        private void Execute_OpenCollectionWindow(Type type)
        {
            Type genericListType = typeof(CollectionViewModel<>).MakeGenericType(type);
            var viewModel = Activator.CreateInstance(genericListType);
            var view = new CollectionWindow(viewModel);
            view.ShowDialog();
        }
    }
}

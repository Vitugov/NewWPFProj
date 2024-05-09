using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFUsefullThings
{
    public class MainViewModel
    {
        public Type DbContextType { get; set; }
        public List<KeyValuePair<string, Type>> TypesToView {  get; set; }
        public List<KeyValuePair<string, ICommand>> OpenCollectionWindowCommand { get; set; } = [];

        public MainViewModel(Type dbContextType)
        {
            DbContextType = dbContextType;
            DbContextCreator.SetDbContextType(DbContextType);
            TypesToView = ClassOverview.TypesForMainWindow;
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
            var viewModel = Activator.CreateInstance(genericListType, DbContextType);
            var view = new CollectionWindow(viewModel);
            view.ShowDialog();
        }
    }
}

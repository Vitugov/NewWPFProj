using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFUsefullThings.ViewModels
{
    public class MainViewModel
    {
        public Type DbContextType { get; set; }
        public List<KeyValuePair<string, Type>> TypesToView {  get; set; }
        public List<KeyValuePair<string, ICommand>> OpenCollectionWindowCommand { get; set; } = [];

        public MainViewModel(Type dbContextType)
        {
            DbContextType = dbContextType;
            TypesToView = ClassOverview.AllDerivedClasses
                .Where(type => type.GetAttribute<SubClassAttribute>() == null
                    || type.GetAttribute<SubClassAttribute>().IsSubClass == false)
                .Select(type => new KeyValuePair<string, Type>(type.GetAttribute<DisplayNamesAttribute>().Plural, type))
                .OrderBy(pair => pair.Key)
                .ToList();
            foreach (var pair in TypesToView)
            {
                var resultCommand = new RelayCommand(obj => Execute_OpenCollectionWindow(pair.Value));
                var resultPair = new KeyValuePair<string, ICommand>(pair.Key, resultCommand);
                OpenCollectionWindowCommand.Add(resultPair);
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

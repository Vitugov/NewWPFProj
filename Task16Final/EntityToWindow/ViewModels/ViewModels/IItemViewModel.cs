using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EntityToWindow.ViewModels
{
    public interface IItemViewModel
    {
        public AllComboDictionary ComboDic { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}

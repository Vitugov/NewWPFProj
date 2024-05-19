using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFUsefullThings
{
    public interface IItemViewModel
    {
        public AllComboDictionary ComboDic { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}

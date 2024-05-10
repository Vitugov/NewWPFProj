using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFUsefullThings
{
    public interface IItemViewModel
    {
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> SubCollectionDic { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}

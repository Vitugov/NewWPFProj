using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFUsefullThings.ViewModels
{
    public interface IItemViewModel
    {
        public Dictionary<string, ObservableCollection<KeyValuePair<string, ProjectModel>>> SubCollectionDic { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}

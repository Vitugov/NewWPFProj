using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFUsefullThings
{
    public interface IItemViewModel<T> : IItemViewModel
        where T : ProjectModel
    {
        public string Header { get; set; }

        public T EditableItem { get; set; }

        public ProjectModel SelectedCollectionItem { get; set; }
    }
}

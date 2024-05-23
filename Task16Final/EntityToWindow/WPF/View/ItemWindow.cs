using EntityToWindow.Core;
using EntityToWindow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EntityToWindow.WPF
{
    public class ItemWindow<T> : BaseItemWindow
        where T : ProjectModel
    {
        public ItemWindow(IItemViewModel<T> context) : base()
        {
            DataContext = context;
            var stackPanel = ItemWindowConstructor.CreateMajorStackPanel(typeof(T));
            var buttonStackPanel = ItemWindowConstructor.BuildButtonStackPanel(this);
            stackPanel.Children.Add(buttonStackPanel);
            SetContent(stackPanel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUsefullThings
{
    public class ItemWindow<T> : BaseItemWindow
        where T : ProjectModel
    {
        public ItemWindow(IItemViewModel<T> context) : base(typeof(T), context)
        {
            DataContext = context;
            Type = typeof(T);
            var stackPanel = ItemWindowConstructor.CreateStackPanel(typeof(T));
            var buttonStackPanel = ItemWindowConstructor.BuildButtonStackPanel(this);
            stackPanel.Children.Add(buttonStackPanel);
            SetContent(stackPanel);
            //ContentStackPanel.Children.Add(stackPanel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFUsefullThings
{
    public static class MainWindowConstructor
    {
        public static StackPanel BuildButtonsStackPanel()
        {
            var stackPanel = new StackPanel();
            for(var i = 0; i< ClassOverview.TypesForMainWindow.Count; i++)
            {
                var button = new Button();

                var contentBinding = new Binding($"OpenCollectionWindowCommand[{i}].Key");
                button.SetBinding(Button.ContentProperty, contentBinding);

                var commandBinding = new Binding($"OpenCollectionWindowCommand[{i}].Value");
                button.SetBinding(Button.CommandProperty, commandBinding);

                stackPanel.Children.Add(button);
            }
            return stackPanel;
        }
    }
}

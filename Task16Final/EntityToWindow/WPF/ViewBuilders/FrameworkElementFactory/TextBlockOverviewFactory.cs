using EntityToWindow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EntityToWindow.WPF
{
    public class TextBlockOverviewFactory : IFrameworkElementFactory
    {
        public int Priority => -1;

        public bool CanHandle(PropertyInfo property) => true;

        public FrameworkElement Create(PropertyInfo property)
        {
            var text = ClassOverview.GetDisplayName(property);
            return new TextBlock
            {
                Width = 100,
                Text = text + ": ",
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }
    }
}

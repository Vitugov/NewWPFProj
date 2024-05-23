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
    public static class FrameworkElementFactory
    {
        private static readonly List<IFrameworkElementFactory> _factories;

        static FrameworkElementFactory()
        {
            _factories =
            [
                new TextBoxFactory(),
                new ComboBoxFactory(),
                new DataGridFactory(),
            ];
        }

        public static FrameworkElement CreateOverview(PropertyInfo property)
        {
            return new TextBlockOverviewFactory().Create(property);
        }
        
        public static FrameworkElement CreateElement(PropertyInfo property)
        {
            var factory = FindFactory(property);
            return factory.Create(property);
        }

        public static StackPanel CreatePanel(PropertyInfo property)
        {
            var factory = FindFactory(property);
            return factory.CreatePanel(property);
        }
        private static IFrameworkElementFactory FindFactory(PropertyInfo property)
        {
            var factory = _factories
                .Where(f => f.CanHandle(property))
                .OrderBy(f => f.Priority)
                .LastOrDefault();
            if (factory == null)
            {
                throw new NotSupportedException($"No factory found for property {property.Name} of type {property.PropertyType}");
            }
            return factory;
        }
    }
}


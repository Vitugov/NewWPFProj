using Castle.Core.Internal;
using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WPFUsefullThings
{
    public static class ItemWindowConstructor
    {
        public static StackPanel CreateStackPanel(Type type)
        {
            StackPanel mainPanel = new StackPanel { Orientation = Orientation.Vertical };
            var properties = type.GetClassOverview().Properties;

            foreach (var property in properties.NotCollection().Concat(properties.IsCollection()).IsVisible())
            {
                StackPanel panel = FrameworkElementFactory.CreatePanel(property);
                mainPanel.Children.Add(panel);
            }
            return mainPanel;
        }

        public static TextBlock CreateTextBlock(PropertyInfo property)
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

        public static StackPanel BuildButtonStackPanel(Window window)
        {
            var viewModel = (IItemViewModel)window.DataContext;
            var saveButton = new Button
            {
                Content = "Сохранить",
                Command = viewModel.SaveCommand,
                CommandParameter = window,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
            };
            var cancelButton = new Button
            {
                Content = "Отмена",
                Command = viewModel.CancelCommand,
                CommandParameter = window,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
            };
            StackPanel buttonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                FlowDirection = FlowDirection.RightToLeft
            };
            buttonStackPanel.Children.Add(saveButton);
            buttonStackPanel.Children.Add(cancelButton);
            return buttonStackPanel;
        }
    }
}

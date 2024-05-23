using EntityToWindow.Core;
using EntityToWindow.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace EntityToWindow.WPF
{
    public static class ItemWindowConstructor
    {
        public static StackPanel CreateMajorStackPanel(Type type)
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

        public static StackPanel BuildButtonStackPanel(Window window)
        {
            var saveButton = CreateSaveButton(window);
            var cancelButton = CreateCancelButton(window);
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

        private static Button CreateSaveButton(Window window)
        {
            var viewModel = (IItemViewModel)window.DataContext;
            return new Button
            {
                Content = "Сохранить",
                Command = viewModel.SaveCommand,
                CommandParameter = window,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
            };
        }

        private static Button CreateCancelButton(Window window)
        {
            var viewModel = (IItemViewModel)window.DataContext;
            return new Button
            {
                Content = "Отмена",
                Command = viewModel.CancelCommand,
                CommandParameter = window,
                Height = 25,
                Width = 100,
                Margin = new Thickness(5)
            };
        }
    }
}

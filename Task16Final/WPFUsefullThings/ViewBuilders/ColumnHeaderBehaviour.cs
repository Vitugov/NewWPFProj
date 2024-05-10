using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

namespace WPFUsefullThings
{
    public class ColumnHeaderBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn += OnGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -= OnGeneratingColumn;
        }

        private static void OnGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
            if (eventArgs.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                // Проверка на наличие атрибута InvisibleAttribute
                var invisibleAttribute = descriptor.Attributes[typeof(InvisibleAttribute)];
                bool isCollection = typeof(IEnumerable).IsAssignableFrom(descriptor.PropertyType) && descriptor.PropertyType != typeof(string);

                if (invisibleAttribute != null || isCollection)
                {
                    // Не генерируем столбец для этого свойства
                    eventArgs.Cancel = true;
                }
                else
                {
                    // Устанавливаем заголовок столбца
                    eventArgs.Column.Header = descriptor.DisplayName ?? descriptor.Name;
                }
                if (eventArgs.PropertyType.IsUserClass())
                {
                    eventArgs.Column.SortMemberPath = $"{eventArgs.PropertyName}.DisplayName";
                    eventArgs.Column.CanUserSort = true;
                }
                if (eventArgs.PropertyType == typeof(DateTime))
                {
                    (eventArgs.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy HH:mm:ss";
                }
            }
        }
    }
}

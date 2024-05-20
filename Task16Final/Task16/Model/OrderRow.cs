using System.ComponentModel;
using EntityToWindow.Core;

namespace Task16.Model
{
    [DisplayNames("Строка заказа", "Номенклатура")]
    [SubClass]
    public partial class OrderRow : ProjectModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        [DisplayName("Наименование товара")]
        public Commodity Commodity { get; set; }
        
        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }

        [DisplayName("Сумма")]
        public decimal Sum => Price * Quantity;
        
        public OrderRow(Commodity commodity, int quantity, decimal price)
        {
            Commodity = commodity;
            Quantity = quantity;
            Price = price;
        }

        public OrderRow() {}

        protected override void UpdateDisplayName()
        {
            DisplayName = $"{Commodity} {Quantity} x {Price} = {Sum}";
        }
    }
}

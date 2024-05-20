using System.Collections.ObjectModel;
using System.ComponentModel;
using EntityToWindow.Core;

namespace Task16.Model
{

    [DisplayNames("Заказ", "Заказы")]
    public partial class Order : ProjectModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        [DisplayName("Клиент")]
        public Client Client { get; set; }

        [DisplayName("Дата и время")]
        public DateTime DateTime { get; set; }

        [DisplayName("Номенклатура")]
        public ObservableCollection<OrderRow> OrderRows { get; set; } = [];


        public Order(Client client, DateTime dateTime, List<OrderRow> orderRowList)
        {
            Client = client;
            DateTime = dateTime;
            OrderRows = new ObservableCollection<OrderRow>(orderRowList);
        }

        public Order() {}

        protected override void UpdateDisplayName()
        {
            DisplayName = $"Заказ {Client} от {DateTime}";
        }
    }
}

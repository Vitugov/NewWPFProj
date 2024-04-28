using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;

namespace Task16.Model
{

    [DisplayNames("Заказ", "Заказы")]
    public class Order : ProjectModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [DisplayName("Клиент")]
        public virtual Client Client { get; set; }

        [DisplayName("Дата и время")]
        public DateTime DateTime { get; set; }

        [DisplayName("Номенклатура")]
        public virtual ObservableCollection<OrderRow> OrderRows { get; set; } = [];


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

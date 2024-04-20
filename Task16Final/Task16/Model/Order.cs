using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;

namespace Task16.Model
{

    [DisplayName("Заказы")]
    public class Order : IProjectModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int? Id { get; set; }

        [DisplayName("Клиент")]
        public Client Client
        { 
            get;
            set;
        }

        [DisplayName("Id товара")]
        public int ProductId
        { 
            get;
            set;
        }

        [DisplayName("Название товара")]
        [StringLength(50, MinimumLength = 2)]
        public string ProductName { get; set; }

        public Order(Client client, int productId, string productName)
        {
            Client = client;
            ProductId = productId;
            ProductName = productName;
        }

        public Order() { }

        public void UpdateFrom(object obj)
        {
            var order = (Order)obj;
            Id = order.Id;
            Client = order.Client;
            ProductId = order.ProductId;
            ProductName = order.ProductName;
        }

        public object Clone()
        {
            return new Order
            {
                Id = Id,
                Client = Client,
                ProductId = ProductId,
                ProductName = ProductName
            };
        }

        public string ToViewString()
        {
            return $"{Client} {ProductId} {ProductName}";
        }

        public override string ToString()
        {
            return ToViewString();
        }
    }
}

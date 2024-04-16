using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task16.Model;
using WPFUsefullThings;

namespace Task16.ViewModel
{
    public class OrderViewModel : INotifyPropertyChangedPlus
    {
        private int? _id;
        private string _email;
        private int _productId;
        private string _productName;

        public OrderViewModel(Order order)
        {
            Id = order.Id;
            Email = order.Email;
            ProductId = order.ProductId;
            ProductName = order.ProductName;
        }

        public OrderViewModel() {}
        public int? Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public int ProductId
        {
            get => _productId;
            set => Set(ref _productId, value);
        }

        public string ProductName
        {
            get => _productName;
            set => Set(ref _productName, value);
        }

        public void UpdateFromOrder(Order order)
        {
            Id = order.Id;
            Email = order.Email;
            ProductId = order.ProductId;
            ProductName = order.ProductName;
        }

        public Order ConvertToOrder()
        {
            return new Order
            {
                Id = this.Id,
                Email = this.Email,
                ProductId = this.ProductId,
                ProductName = this.ProductName
            };
        }

        public void UpdateOrder(Order order)
        {
            order.Id = Id;
            order.Email = Email;
            order.ProductId = ProductId;
            order.ProductName = ProductName;
        }
    }
}

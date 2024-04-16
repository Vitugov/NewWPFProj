using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task16.Model;
using WPFUsefullThings;

namespace Task16.ViewModel
{
    public class OrderValidationViewModel : INotifyPropertyChangedPlus
    {
        private bool _id;
        private bool _email;
        private bool _productId;
        private bool _productName;

        public bool Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        public bool Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public bool ProductId
        {
            get => _productId;
            set => Set(ref _productId, value);
        }

        public bool ProductName
        {
            get => _productName;
            set => Set(ref _productName, value);
        }

        public OrderValidationViewModel() { }

        public IEnumerable<bool> GetAsBoolSet()
        {
            return [Email, ProductId, ProductName];
        }

        public bool IsValid()
        {
            return !GetAsBoolSet().Any(e => e == false);
        }
    }
}

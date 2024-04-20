using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using Task16.Model;
using Task16.View;
using WPFUsefullThings;

namespace Task16.ViewModel
{
    public class OrderVM : INotifyPropertyChangedPlus
    {
        public Order OriginalOrder;
        public readonly bool IsNew;


        private OrderViewModel _OrderView;
        public OrderViewModel OrderView
        {
            get => _OrderView;
            set => Set(ref _OrderView, value);
        }

        private OrderValidationViewModel _Validation;
        public OrderValidationViewModel Validation
        {
            get => _Validation;
            set => Set(ref _Validation, value);
        }

        private Dictionary<string, string> _ClientsEmailDic;
        public Dictionary<string, string> ClientsEmailDic
        {
            get => _ClientsEmailDic;
            set => Set(ref _ClientsEmailDic, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public OrderVM(Order? order, Client? client, Dictionary<string, string> clientsEmailDic)
        {
            SubscribeToPropertyChanged(OrderVM_PropertyChanged);

            IsNew = order == null;
            OriginalOrder = order == null ? new Order() : order;
            
            ClientsEmailDic = clientsEmailDic;

            Validation = new OrderValidationViewModel();
            Validation.SubscribeToPropertyChanged(OrderVM_PropertyChanged);
            OrderView = new OrderViewModel();
            OrderView.SubscribeToPropertyChanged(OrderVM_PropertyChanged);
            OrderView.UpdateFromOrder(OriginalOrder);

            SaveCommand = new RelayCommand(obj => ExecuteSaveCommand(obj));
            CancelCommand = new RelayCommand(obj => CloseWindow(obj));
        }

        private void ExecuteSaveCommand(object obj)
        {
            if (!Validation.IsValid())
            {
                ValidationRules.ShowErrorMessage();
                return;
            }

            using (var db = new SqliteContext())
            {
                OrderView.UpdateOrder(OriginalOrder);
                db.Orders.Update(OriginalOrder);
                db.SaveChanges();
            }
            CloseWindow(obj);
        }

        private void CloseWindow(object obj) => (obj as Window).Close();

        private void OrderVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderView.Email))
            {
                Validation.Email = OrderView.Email.ValidateEmail();
            }
            if (e.PropertyName == nameof(OrderView.ProductId))
            {
                Validation.ProductId = OrderView.ProductId.ValidateMoreThanZero();
            }
            if (e.PropertyName == nameof(OrderView.ProductName))
            {
                Validation.ProductName = OrderView.ProductName.ValidateNotEmptyString();
            }
        }
    }
}

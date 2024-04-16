using WPFUsefullThings;
using Task16.Other;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Input;
using Task16.View;
using System.Windows;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Task16.Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Task16.ViewModel
{

    public class MainWindowVM : INotifyPropertyChangedPlus
    {
        public SqliteContext Db = new SqliteContext();

        private ObservableCollection<Client> _Clients;
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
        }
        public ObservableCollection<Order> _Orders;
        public ObservableCollection<Order> Orders
        {
            get => _Orders;
            set => Set(ref _Orders, value);
        }

        public ListCollectionView _OrdersView;

        public ListCollectionView OrdersView
        {
            get => _OrdersView;
            set => Set(ref _OrdersView, value);
        }


        private Client? _SelectedClient;
        public Client? SelectedClient
        {
            get => _SelectedClient;
            set => Set(ref _SelectedClient, value);
        }

        private Order? _SelectedOrder;
        public Order? SelectedOrder
        {
            get => _SelectedOrder;
            set => Set(ref _SelectedOrder, value);
        }

        private bool _IsAllOrdersVisible = true;
        public bool IsAllOrdersVisible
        {
            get => _IsAllOrdersVisible;
            set => Set(ref _IsAllOrdersVisible, value);
        }

        public ICommand AddNewClientCommand { get; }
        public ICommand ChangeClientCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ICommand AddNewOrderCommand { get; }
        public ICommand ChangeOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

        public MainWindowVM()
        {
            SubscribeToPropertyChanged(MainWindowVM_PropertyChanged);
            Init();

            using (Db)
            {
                Clients = new ObservableCollection<Client>(Db.Clients);
                Orders = new ObservableCollection<Order>(Db.Orders);
            }

            AddNewClientCommand = new RelayCommand(obj => ExecuteChangeClient());
            ChangeClientCommand = new RelayCommand(obj => ExecuteChangeClient(SelectedClient), obj => SelectedClient != null);
            DeleteClientCommand = new RelayCommand(obj => ExecuteDeleteClient(SelectedClient), obj => SelectedClient != null);

            AddNewOrderCommand = new RelayCommand(obj => ExecuteChangeOrder(null, SelectedClient));
            ChangeOrderCommand = new RelayCommand(obj => ExecuteChangeOrder(SelectedOrder), obj => SelectedOrder != null);
            DeleteOrderCommand = new RelayCommand(obj => ExecuteDeleteOrder(SelectedOrder), obj => SelectedOrder != null);
        }

        private void Init()
        {
            using (Db)
            {
                if (!Db.Clients.Any())
                {
                    Initialization.Instance.Init();
                }
            }
        }

        public void RefreshView(object sender, EventArgs e)
        {
            RefreshClients();
            RefreshOrders();
        }

        private void RefreshOrders()
        {
            if (IsAllOrdersVisible)
            {
                OrdersView.Filter = obj => true;
                return;
            }
            if (SelectedClient == null)
            {
                OrdersView.Filter = obj => (obj as Order).Email == "";
                return;
            }
            else
            {
                OrdersView.Filter = obj => (obj as Order).Email == SelectedClient.Email;
            }
        }

        private void RefreshClients()
        {
            using (Db)
            {
                Clients = new ObservableCollection<Client>(Db.Clients);
                Orders = new ObservableCollection<Order>(Db.Orders);
            }
        }

        private void ExecuteChangeClient(Client? client = null)
        {
            var clientVM = new ClientVM(client);
            var clientView = new ClientView(clientVM);
            clientView.ShowDialog();
        }

        private void ExecuteDeleteClient(Client client)
        {
            if (Orders.Any(order => order.Email == client.Email))
            {
                MessageBox.Show("Нельзя удалить клиента у которого есть заказы", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            Clients.Remove(client);

            using (var db = new SqliteContext())
            {
                db.Clients.Remove(client);
                db.SaveChanges();
            }
        }

        private void ExecuteChangeOrder(Order? order = null, Client? client = null)
        {
            var clientsDic = Clients.GetClientsEmailDictionary();
            var orderVM = new OrderVM(order, client, clientsDic);
            var orderView = new OrderView(orderVM);
            orderView.ShowDialog();
        }

        private void ExecuteDeleteOrder(Order order)
        {
            Orders.Remove(order);
            
            using (var db = new SqliteContext())
            {
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }

        private void MainWindowVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedClient) || e.PropertyName == nameof(IsAllOrdersVisible))
            {
                RefreshOrders();
            }
            if (e.PropertyName == nameof(Orders))
            {
                OrdersView = new ListCollectionView(Orders);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Task16.Model;
using WPFUsefullThings;
using WPFUsefullThings.Validation;

namespace Task16.ViewModel
{
    public class ClientVM : INotifyPropertyChangedPlus
    {        
        public Client OriginalClient;
        public readonly bool IsNew;


        private ClientViewModel _ClientView;
        public ClientViewModel ClientView
        {
            get => _ClientView;
            set => Set(ref _ClientView, value);
        }

        private ClientValidationViewModel _Validation;
        public ClientValidationViewModel Validation
        {
            get => _Validation;
            set => Set(ref _Validation, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ClientVM(Client? client)
        {
            SubscribeToPropertyChanged(ClientVM_PropertyChanged);

            IsNew = (client == null);
            OriginalClient = client == null ? new Client() : client;
            Validation = new ClientValidationViewModel();
            Validation.SubscribeToPropertyChanged(ClientVM_PropertyChanged);
            ClientView = new ClientViewModel();
            ClientView.SubscribeToPropertyChanged(ClientVM_PropertyChanged);
            ClientView.UpdateFromClient(OriginalClient);


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
                ClientView.UpdateClient(OriginalClient);
                db.Clients.Update(OriginalClient);
                db.SaveChanges();
            }
            CloseWindow(obj);
        }

        internal void CloseWindow(object obj) => (obj as Window).Close();

        public void ClientVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Surname")
            {
                Validation.Surname = ClientView.Surname.ValidateNotEmptyString();
            }
            if (e.PropertyName == "FirstName")
            {
                Validation.FirstName = ClientView.FirstName.ValidateNotEmptyString();
            }
            if (e.PropertyName == "Patronymic")
            {
                Validation.Patronymic = ClientView.Patronymic.ValidateNotEmptyString();
            }
            if (e.PropertyName == "TelephoneNumber")
            {
                Validation.TelephoneNumber = true;
            }
            if (e.PropertyName == "Email")
            {
                Validation.Email = ClientView.Email.ValidateEmail();
            }
        }
    }
}

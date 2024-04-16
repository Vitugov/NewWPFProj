using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task16.Model;
using WPFUsefullThings;

namespace Task16.ViewModel
{
    public class ClientViewModel : INotifyPropertyChangedPlus
    {
        private int? _id;
        private string _surname;
        private string _firstName;
        private string _patronymic;
        private string? _telephoneNumber;
        private string _email;

        public ClientViewModel(Client? client = null)
        {
            if (client != null)
            {
                Id = client.Id;
                Surname = client.Surname;
                FirstName = client.FirstName;
                Patronymic = client.Patronymic;
                TelephoneNumber = client.TelephoneNumber;
                Email = client.Email;
            }
            else
            {
                Id = null;
                Surname = "";
                FirstName = "";
                Patronymic = "";
                TelephoneNumber = "";
                Email = "";
            }
        }

        public ClientViewModel()
        {

        }

        public int? Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        public string Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public string Patronymic
        {
            get => _patronymic;
            set => Set(ref _patronymic, value);
        }

        public string? TelephoneNumber
        {
            get => _telephoneNumber;
            set => Set(ref _telephoneNumber, value);
        }

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public void UpdateFromClient(Client client)
        {
            Id = client.Id;
            Surname = client.Surname;
            FirstName = client.FirstName;
            Patronymic = client.Patronymic;
            TelephoneNumber = client.TelephoneNumber;
            Email = client.Email;
        }

        public Client ConvertToClient()
        {
            return new Client
            {
                Id = this.Id,
                Surname = this.Surname,
                FirstName = this.FirstName,
                Patronymic = this.Patronymic,
                TelephoneNumber = this.TelephoneNumber,
                Email = this.Email
            };
        }

        public void UpdateClient(Client client)
        {
            client.Id = Id;
            client.Surname = Surname;
            client.FirstName = FirstName;
            client.Patronymic = Patronymic;
            client.TelephoneNumber = TelephoneNumber;
            client.Email = Email;
        }
    }
}

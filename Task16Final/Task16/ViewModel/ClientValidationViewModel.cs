using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;

namespace Task16.ViewModel
{
    public class ClientValidationViewModel : INotifyPropertyChangedPlus
    {
        private bool _id;
        private bool _surname;
        private bool _firstName;
        private bool _patronymic;
        private bool _telephoneNumber;
        private bool _email;

        public bool Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        public bool Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        public bool FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        public bool Patronymic
        {
            get => _patronymic;
            set => Set(ref _patronymic, value);
        }

        public bool TelephoneNumber
        {
            get => _telephoneNumber;
            set => Set(ref _telephoneNumber, value);
        }

        public bool Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public ClientValidationViewModel() {}

        public IEnumerable<bool> GetAsBoolSet()
        {
            return [ Surname, FirstName, Patronymic, TelephoneNumber, Email ];
        }
        public bool IsValid()
        {
            return !GetAsBoolSet().Any(e => e == false);
        }
    }
}

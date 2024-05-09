using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WPFUsefullThings;

namespace Task16.Model
{
    [DisplayNames("Клиент", "Клиенты")]
    public partial class Client : ProjectModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        [DisplayName("Фамилия")]
        [StringLength(25, MinimumLength = 1)]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        [StringLength(25, MinimumLength = 1)]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        [StringLength(25, MinimumLength = 1)]
        public string Patronymic { get; set; }

        [DisplayName("Номер телефона")]
        public string? TelephoneNumber { get; set; }

        [DisplayName("E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        public Client(string surname, string firstName, string patronymic, string? telephoneNumber, string email)
        {
            Surname = surname;
            FirstName = firstName;
            Patronymic = patronymic;
            TelephoneNumber = telephoneNumber;
            Email = email;
        }
        public Client() {}

        
        public string ToViewString()
        {
            return $"{Surname} {FirstName} {Patronymic}";
        }

        protected override void UpdateDisplayName()
        {
            DisplayName = $"{Surname} {FirstName} {Patronymic}";
        }
    }
}

using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Task16.Model
{
    [DisplayNames("Клиент", "Клиенты")]
    public class Client : ProjectModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public Client() { }

        
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

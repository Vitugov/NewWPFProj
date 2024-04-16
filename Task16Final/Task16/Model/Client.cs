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
    public class Client : IUpdateable<Client>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int? Id { get; set; }
        
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Surname { get; set; }
        
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string Patronymic { get; set; }
        public string? TelephoneNumber { get; set; }
        
        [Required]
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

        public object Clone()
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

        public void UpdateFrom(Client obj)
        {
            Id = obj.Id;
            Surname = obj.Surname;
            FirstName = obj.FirstName;
            Patronymic = obj.Patronymic;
            TelephoneNumber = obj.TelephoneNumber;
            Email = obj.Email;
        }
    }
}

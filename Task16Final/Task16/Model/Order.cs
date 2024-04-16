using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task16.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Order
    {
        public int? Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ProductName { get; set; }

        public Order(string email, int productId, string productName)
        {
            Email = email;
            ProductId = productId;
            ProductName = productName;
        }

        public Order() { }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;

namespace Task16.Model
{
    [DisplayNames("Товар","Товары")]
    public class Commodity : ProjectModel
    {
        [DisplayName("Название товара")]
        [StringLength(50, MinimumLength = 2)]
        public string ProductName { get; set; }
        protected override void UpdateDisplayName()
        {
            DisplayName = ProductName;
        }
    }
}

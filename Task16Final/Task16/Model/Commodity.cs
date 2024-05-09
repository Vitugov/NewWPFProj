using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WPFUsefullThings;

namespace Task16.Model
{
    [DisplayNames("Товар","Товары")]
    public partial class Commodity : ProjectModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler? PropertyChanged;

        [DisplayName("Название товара")]
        [StringLength(50, MinimumLength = 2)]
        public string ProductName { get; set; }
        
        public Commodity(string  productName)
        {
            ProductName = productName;
        }

        public Commodity() {}
        protected override void UpdateDisplayName()
        {
            DisplayName = ProductName;
        }
    }
}

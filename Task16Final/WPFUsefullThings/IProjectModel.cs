using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public interface IProjectModel : ICloneable, INotifyPropertyChanged
    {
        public int? Id { get; set; }
        public void UpdateFrom(object obj);
        public string ToViewString();
    }
}

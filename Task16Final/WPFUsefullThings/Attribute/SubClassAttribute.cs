using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class SubClassAttribute : Attribute
    {
        public bool IsSubClass { get; set; }

        public SubClassAttribute(bool isSubClass = true)
        {
            IsSubClass = isSubClass;
        }
    }
}

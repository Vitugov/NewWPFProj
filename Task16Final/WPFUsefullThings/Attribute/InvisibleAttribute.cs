using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class InvisibleAttribute : Attribute
    {
        public bool IsInvisible { get; set; }

        public InvisibleAttribute(bool isInvivsible = true)
        {
            IsInvisible = isInvivsible;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public interface IUpdateable<T> : ICloneable
    {
        public void UpdateFrom(T obj);
    }
}

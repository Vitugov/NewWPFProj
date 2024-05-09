using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.HadrData;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPFUsefullThings
{
    public static class Extensions
    {
        public static bool IsContainingString(this object obj, string str)
        {
            var objectProperties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (string.IsNullOrEmpty(str))
                return true;
            var result = objectProperties
                        .Any(property =>
                        {
                            var propertyValue = property.GetValue(obj);
                            return propertyValue != null && propertyValue.ToString().ToLower().Contains(str.ToLower());
                        });

            return result;
        }


    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public static class DbContextCreator
    {
        private static Type _dbContextType;
        public static DbContext Create()
        {
            if (_dbContextType == null)
            {
                throw new NullReferenceException("field '_dbContextType' has no value assigned");
            }
            return (DbContext)Activator.CreateInstance(_dbContextType);
        }
        public static void SetDbContextType(Type dbContextType)
        {
            _dbContextType = dbContextType;
        }
    }
}

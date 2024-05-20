using Microsoft.EntityFrameworkCore;

namespace EntityToWindow.DbActions
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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings.DbHandler
{
    public class DbHandler
    {
        public void ExecuteQuery(IQueryable querry, DbContext context)
        {

        }

        public void GetSet(Type type)
        {
            using (var context = DbContextCreator.Create())
            {
                var set = context.Set(type);

            }


        }
}

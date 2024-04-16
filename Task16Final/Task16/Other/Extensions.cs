using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Runtime.CompilerServices;
using Task16.Model;

namespace Task16.Other
{
    public static class Extensions
    {
        //public static void AddMany(this IDataParameterCollection list, DbParameter[] parameters)
        //{
        //    foreach (var param in parameters)
        //    {
        //        list.Add(param);
        //    }
        //}

        public static void AddMany(this IDataParameterCollection targetParameters, params DbParameter[] sourceParameters)
        {
            foreach (var param in sourceParameters)
            {
                var clonedParam = param.CloneParameter();
                targetParameters.Add(clonedParam);
            }
        }

        private static DbParameter CloneParameter(this DbParameter sourceParameter)
        {
            return (DbParameter)((ICloneable)sourceParameter).Clone();
        }

        public static DbCommand MakeDbCommand<T>(this string dbCommand, T connection)
            where T : DbConnection
        {
            if (connection.GetType() == typeof(OleDbConnection))
            {
                return new OleDbCommand(dbCommand, connection as OleDbConnection);
            }
            else if (connection.GetType() == typeof(SqlConnection))
            {
                return new SqlCommand(dbCommand, connection as SqlConnection);
            }
            else 
            {
                throw new ArgumentException();
            }
        }

        public static Dictionary<string, string> GetClientsEmailDictionary(this IEnumerable<Client> clients)
        {
            var resultList = new Dictionary<string, string>();
            foreach (Client client in clients)
            {
                var clientString = $"{client.Email} ({client.Surname} {client.FirstName} {client.Patronymic})";
                resultList[clientString] = client.Email;
            }
            return resultList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Lib
{
    class ExistingUsername
    {
        public static bool Check(string username)
        {
            string query = $"SELECT * FROM dbo.Player WHERE (Username = '{username}');";
            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
            SqlDataReader sqlreader = sqlcmd.ExecuteReader();
            if (sqlreader.Read()) //exist username
            {
                sqlreader.Close();
                return true;
            }
            sqlreader.Close();
            return false;

        }
    }
}

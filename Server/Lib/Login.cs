using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;

namespace Server.Lib
{
    class Login
    {
        public static bool Do(LoginInfo data)
        {
            string query = $"SELECT Username, Password FROM dbo.Player WHERE (Username = '{data.username}') AND (Password = '{data.password}');";
            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
            SqlDataReader sqlreader = sqlcmd.ExecuteReader();
            if (sqlreader.Read())
            {
                sqlreader.Close();
                return true;
            }
            sqlreader.Close();
            return false;
        }
    }
}

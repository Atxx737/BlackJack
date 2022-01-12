using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Lib
{
    class JoinRoom
    {
        public static bool Do(string data)
        {
            string query = $"Select RoomID from dbo.Room where RoomID = '{data}';";
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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Lib
{
    class ExistingRoomID
    {
            public static bool Check(string roomid)
            {
                string query = $"SELECT * FROM dbo.Room WHERE (RoomID = '{roomid}');";
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

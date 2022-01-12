using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
namespace Server.Lib
{
    class CreateRoom
    {
        public static bool Do(RoomInfo data)
        {
            string query = "INSERT INTO dbo.Room (RoomID , IPAddress, Port, MinBet) VALUES "
                                                    + $"('{data.RoomID}','{data.IpAddress}','{data.Port}','{data.Minbet}');";
            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
            try
            {
                SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                sqlreader.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

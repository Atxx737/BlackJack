using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace Server.Lib
{
    class Register
    {
        public static bool Do(RegisterInfo data)
        {
            string query = "declare @result nvarchar(5); EXEC dbo.sp_Player_TusinhID @result OUT; " + "INSERT INTO dbo.Player (ID,Playername, Username, Password, Money) VALUES "
                                                    + $"(@result,'{data.playername}','{data.username}','{data.password}','10000');";
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

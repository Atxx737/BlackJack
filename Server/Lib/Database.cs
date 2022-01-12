using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Server.Lib
{
    public class ServerInfo
    {
        public static string connstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\Documents\Blackjack.mdf;Integrated Security=True;Connect Timeout=30";

        public static SqlConnection connection;

        public static bool OpenConnection()
        {
            try
            {
                connection = new SqlConnection(connstring);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }

        }
        //kết nối server
        public static void OpenConnect()
        {
            bool openconn = OpenConnection();
            if (openconn == false)
            {
                if ((MessageBox.Show("Kết nối đến máy chủ thất bại, thử lại?", "Thử lại", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1) == DialogResult.Yes) == true)
                {
                    OpenConnect();
                }
                else
                {
                    Environment.Exit(0);
                }

            }
        }
    }
}

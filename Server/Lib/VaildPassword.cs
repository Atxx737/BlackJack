using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Server.Lib
{
    public class VaildPassword
    {
        public static bool Check(string username, string password)
        {
            string query = $"SELECT Password FROM dbo.UserAccount WHERE Username = '{username}'";
            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
            SqlDataReader sqlreader = sqlcmd.ExecuteReader();
            if (sqlreader.Read())
            {
                string passwordcomfirm = sqlreader["Password"].ToString();
                if (string.Equals(password, passwordcomfirm) == false)
                {
                    sqlreader.Close();
                    MessageBox.Show("Mật khẩu cũ không chính xác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    sqlreader.Close();
                    return true;
                }
            }
            sqlreader.Close();
            return false;

        }
    }
}

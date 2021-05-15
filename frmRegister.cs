using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clients.Lib;
using Lib;

namespace Clients
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            string playerName = textBox1.Text;
            string userName = textBox2.Text;
            string password = textBox3.Text;
            string rePassword = textBox4.Text;

            if (playerName.Length < 3)
            {
                MessageBox.Show("Tên người chơi tối thiểu phải 3 kí tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else if (userName.Length < 5)
            {
                MessageBox.Show("Tài khoản tối thiểu phải 5 kí tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
            }
            else if (password.Length < 8)
            {
                MessageBox.Show("Mật khẩu tối thiểu phải 8 kí tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
            }
            else if (string.Equals(password, rePassword) == false)
            {
                MessageBox.Show("Xác nhận mật khẩu không đúng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Clear();
                textBox4.Focus();
            }
            else
            {
                RegisterInfo regInfor = new RegisterInfo();
                regInfor.playername = playerName;
                regInfor.username = userName;
                regInfor.password = password;
                SocketData data = new SocketData((int)SocketCommand.Register, regInfor);

                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress IP = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipEp = new IPEndPoint(IP, 800);

                try
                {
                    tcpClient.Connect(ipEp);
                }
                catch
                {
                    MessageBox.Show("Không thể kết nối đến server", "Lỗi",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                byte[] sendBuffer = new byte[2048];
                DataManager dataManager = new DataManager();
                sendBuffer = dataManager.SerializeData(data);

                tcpClient.Send(sendBuffer);
                tcpClient.Shutdown(SocketShutdown.Send);

                byte[] receiveBuffer = new byte[2048];
                tcpClient.Receive(receiveBuffer);
                string result = (string)dataManager.DeserializeData(receiveBuffer);

                if (string.Compare(result, "rs") == 0)
                {
                    MessageBox.Show($"Đăng kí thành công! Thông tin tài khoản: \nTên người chơi: {playerName}\nTài khoản: {userName}\nBây giờ bạn có thể đăng nhập bằng tài khoản này.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmLogin frm = new frmLogin();
                    frm.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập đã được sử dụng.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                tcpClient.Close();

            }
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {

        }
    }
}

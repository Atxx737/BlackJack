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
using Lib;
using Clients.Lib;

namespace Clients
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmRegister().Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 2 || textBox2.Text.Length < 1)
            {
                MessageBox.Show("Chưa nhập tài khoản và mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoginInfo logInfo = new LoginInfo(textBox1.Text, textBox2.Text);
            SocketData data = new SocketData((int)SocketCommand.Login, logInfo);

            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(IP, 800);

            try
            {
                tcpClient.Connect(ipEp);
            }
            catch
            {

            }

            byte[] sendBuffer = new byte[2048];
            DataManager dataManager = new DataManager();
            sendBuffer = dataManager.SerializeData(data);

            tcpClient.Send(sendBuffer);
            tcpClient.Shutdown(SocketShutdown.Send);

            byte[] receiveBuffer = new byte[2048];
            tcpClient.Receive(receiveBuffer);
            SocketData result = (SocketData) dataManager.DeserializeData(receiveBuffer);
            if (result.command == 1)
            {
                MessageBox.Show($"Đăng nhập thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmStart frm = new frmStart((UserInfo) result.obJect);
                frm.Show();
            }
            else
            {
                MessageBox.Show($"Tài khoản hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

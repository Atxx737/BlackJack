using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clients.Lib;
using System.Net.Sockets;
using System.Net;
using Lib;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Clients
{
    public partial class frmRank : Form
    {
        public frmRank()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SocketData data = new SocketData((int)SocketCommand.RankDetail, "");

            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(IP, 800);

            try
            {
                tcpClient.Connect(ipEp);
            }
            catch
            {
                MessageBox.Show("Không thể kết nối tới server", "Lỗi");
            }

            byte[] sendBuffer = new byte[2048];
            DataManager dataManager = new DataManager();
            sendBuffer = dataManager.SerializeData(data);

            tcpClient.Send(sendBuffer);
            tcpClient.Shutdown(SocketShutdown.Send);

            byte[] receiveBuffer = new byte[1024 * 5000];
            tcpClient.Receive(receiveBuffer);

            List<UserInfo> result = (List<UserInfo>) dataManager.DeserializeData(receiveBuffer);

            label1.Text = result[0].playername;
            label2.Text = result[0].Money;
            label7.Text = result[0].ID;
            label3.Text = result[1].playername;
            label4.Text = result[1].Money;
            label8.Text = result[1].ID;
            label5.Text = result[2].playername;
            label6.Text = result[2].Money;
            label9.Text = result[2].ID;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

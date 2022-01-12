using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;
using Clients.Lib;
using System.Net.Sockets;
using System.Net;
using Extensions;
using Clients.Game;

namespace Clients
{
    public partial class frmStart : Form
    {
        UserInfo userInfo;
        public frmStart(UserInfo data)
        {
            userInfo = data;
            InitializeComponent();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {
            textBox2.Text = "Xin chào " + userInfo.playername + "!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain frm = new frmMain(userInfo);
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmRank frm = new frmRank();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCreRoom frm = new frmCreRoom(userInfo);
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 4 || textBox1.Text == "")
            {
                MessageBox.Show("Mã phòng phải bao gồm 4 ký tự", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else if (textBox1.Text.IsAlphaNumeric(true, false) == false)
            {
                MessageBox.Show("Mã phòng chứa ký tự không hợp lệ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else
            {
                string roomID = textBox1.Text;
                SocketData data = new SocketData((int)SocketCommand.Join, roomID);

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

                byte[] receiveBuffer = new byte[2048];
                tcpClient.Receive(receiveBuffer);
                tcpClient.Shutdown(SocketShutdown.Receive);
                SocketData result = (SocketData)dataManager.DeserializeData(receiveBuffer);
                


                if (result.command == 1)
                {
                    RoomInfo roomInfo = (RoomInfo)result.obJect;
                    Socket tcpClient1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress IP1 = IPAddress.Parse(roomInfo.IpAddress);
                    IPEndPoint ipEp1 = new IPEndPoint(IP, roomInfo.Port);

                    try
                    {
                        tcpClient1.Connect(ipEp1);
                        int cmd = 1;

                        GameControll data1 = new GameControll(cmd, userInfo);
                        byte[] sendBuffer1 = new byte[2048];
                        sendBuffer1 = dataManager.SerializeData(data1);

                        tcpClient1.Send(sendBuffer1);
                        tcpClient1.Shutdown(SocketShutdown.Send);

                        byte[] receiveBuffer1 = new byte[2048];
                        tcpClient1.Receive(receiveBuffer1);
                        DataManager dataManager1 = new DataManager();

                        GameControll result1 = (GameControll)dataManager1.DeserializeData(receiveBuffer1);
                        if (result1.controll == 0)
                        {
                            MessageBox.Show("Sòng đã đầy", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (result1.controll == 1)
                        {
                            MessageBox.Show($"Vào sòng thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            UserInfo dealerInfo = (UserInfo)result1.obj;                            
                            frmChallenger frm = new frmChallenger(roomInfo, userInfo, dealerInfo);
                            this.Hide();
                            frm.Show();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Không thể kết nối tới server", "Lỗi");
                    }                  
                }
                else
                {
                    MessageBox.Show($"Sòng không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmInfo frm = new frmInfo(userInfo);
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Bạn có thực sự muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1) == DialogResult.Yes) == true)
            {
                DataManager dataManager = new DataManager();
                dataManager.LogOut(userInfo.username);
                Hide();
                frmLogin frm = new frmLogin();
                frm.Show();
                frm.Focus();
            }
        }

        private void frmStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager dataManager = new DataManager();
            dataManager.LogOut(userInfo.username);
        }
    }
}

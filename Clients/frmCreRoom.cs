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
using Extensions;

namespace Clients
{
    public partial class frmCreRoom : Form
    {
        UserInfo userInfo;
        public frmCreRoom(UserInfo data)
        {
            userInfo = data;
            InitializeComponent();
        }

        private void frmCreRoom_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsIpV4Address(textBox1.Text.Trim()) == false)
            {
                MessageBox.Show("Địa chỉ IP không hợp lệ.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else if (textBox2.Text.All(Char.IsDigit) == false || textBox2.Text == "")
            {
                MessageBox.Show("Port không hợp lệ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
            }
            else if(textBox3.Text.All(Char.IsDigit) == false || textBox3.Text == "")
            {
                MessageBox.Show("Tiền cược tối thiểu phải là số nguyên không âm", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
            }
            else if(textBox4.Text.Length != 4 || textBox4.Text == "")
            {
                MessageBox.Show("Mã phòng phải bao gồm 4 ký tự", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
            }
            else if(textBox4.Text.IsAlphaNumeric(true, false) == false)
            {
                MessageBox.Show("Mã phòng chứa ký tự không hợp lệ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
            }
            else
            {
                try
                {
                    Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint testIpEp = new IPEndPoint(IPAddress.Parse(textBox1.Text), Int32.Parse(textBox2.Text));
                    listenerSocket.Bind(testIpEp);
                    listenerSocket.Close();

                    RoomInfo roomInfo = new RoomInfo(textBox1.Text.Trim(), Int32.Parse(textBox2.Text.Trim()), textBox4.Text.Trim(), Int32.Parse(textBox3.Text.Trim()));
                    SocketData data = new SocketData((int)SocketCommand.CreateRoom, roomInfo);

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
                    string result = (string)dataManager.DeserializeData(receiveBuffer);

                    if (string.Compare(result, "rs") == 0)
                    {
                        MessageBox.Show($"Tạo phòng thành công! Thông tin phòng: \nMã phòng: {roomInfo.RoomID}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmDealer frm = new frmDealer(userInfo, roomInfo);
                        frm.Show();
                        Hide();
                    }
                    if (string.Compare(result, "re") == 0)
                    {
                        MessageBox.Show("Mã phòng đã tồn tại.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (string.Compare(result, "-1") == 0)
                    {
                        MessageBox.Show("Đã có lỗi xảy ra, xin vui lòng thử lại", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    tcpClient.Close();
                }
                catch (Exception x)
                {
                    MessageBox.Show("Port không thể sử dụng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                
            }
            
        }

        public static bool IsIpV4Address(string str)
        {
            string[] arr = str.Split('.');
            if (arr.Length != 4)
                return false;
            foreach (string sub in arr)
            {
                try
                {
                    int test = int.Parse(sub);
                    if (test < 0 || test > 255)
                        return false;

                    //nếu sub có nhiều chữ số 0 ở trước, khi convert qua số sẽ bị mất các số 0 ở đằng trước  
                    if (test.ToString().Length != sub.Length)
                    {
                        // chứng tỏ sub có chứa các số 0 thừa ở đằng trước -> không hợp lệ.  
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            // vượt qua đc hết đống thử thách này thì chắc chắn là địa chỉ Ip V4 rùi ^^!    
            return true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmStart frm = new frmStart(userInfo);
            this.Hide();
            frm.Show();
        }

        private void frmCreRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmCreRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager dataManager = new DataManager();
            dataManager.LogOut(userInfo.username);
        }
    }
}

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
using Clients.Game;
using Clients.Lib;
using Lib;
using System.Threading;

namespace Clients
{
    public partial class frmChallenger : Form
    {
        Player dealer;
        Player player; 
        RoomInfo roomInfo { get; set; }
        UserInfo userInfo { get; set; }
        UserInfo dealerInfo { get; set; }
        MoneyChange moneyChange;
        int MinBet;
        int betMoney;
        int uWin = 0;
        public frmChallenger(RoomInfo room, UserInfo user, UserInfo dealer)
        {
            roomInfo = room;
            userInfo = user;
            dealerInfo = dealer;
            MoneyChange term = new MoneyChange(userInfo.ID, userInfo.Money);
            moneyChange = term;
            MinBet = roomInfo.Minbet;
            InitializeComponent();
        }

        public PictureBox ShowCard(string id)
        {
            PictureBox pb = new PictureBox();
            pb.Visible = false;
            pb.Width = 71;
            pb.Height = 96;

            switch (id)
            {
                case "1C":
                    pb.Image = Properties.Resources._1C;
                    break;
                case "1R":
                    pb.Image = Properties.Resources._1R;
                    break;
                case "1H":
                    pb.Image = Properties.Resources._1H;
                    break;
                case "1B":
                    pb.Image = Properties.Resources._1B;
                    break;
                case "2C":
                    pb.Image = Properties.Resources._2C;
                    break;
                case "2R":
                    pb.Image = Properties.Resources._2R;
                    break;
                case "2H":
                    pb.Image = Properties.Resources._2H;
                    break;
                case "2B":
                    pb.Image = Properties.Resources._2B;
                    break;
                case "3C":
                    pb.Image = Properties.Resources._3C;
                    break;
                case "3R":
                    pb.Image = Properties.Resources._3R;
                    break;
                case "3H":
                    pb.Image = Properties.Resources._3H;
                    break;
                case "3B":
                    pb.Image = Properties.Resources._3B;
                    break;
                case "4C":
                    pb.Image = Properties.Resources._4C;
                    break;
                case "4R":
                    pb.Image = Properties.Resources._4R;
                    break;
                case "4H":
                    pb.Image = Properties.Resources._4H;
                    break;
                case "4B":
                    pb.Image = Properties.Resources._4B;
                    break;
                case "5C":
                    pb.Image = Properties.Resources._5C;
                    break;
                case "5R":
                    pb.Image = Properties.Resources._5R;
                    break;
                case "5H":
                    pb.Image = Properties.Resources._5H;
                    break;
                case "5B":
                    pb.Image = Properties.Resources._5B;
                    break;
                case "6C":
                    pb.Image = Properties.Resources._6C;
                    break;
                case "6R":
                    pb.Image = Properties.Resources._6R;
                    break;
                case "6H":
                    pb.Image = Properties.Resources._6H;
                    break;
                case "6B":
                    pb.Image = Properties.Resources._6B;
                    break;
                case "7C":
                    pb.Image = Properties.Resources._7C;
                    break;
                case "7R":
                    pb.Image = Properties.Resources._7R;
                    break;
                case "7H":
                    pb.Image = Properties.Resources._7H;
                    break;
                case "7B":
                    pb.Image = Properties.Resources._7B;
                    break;
                case "8C":
                    pb.Image = Properties.Resources._8C;
                    break;
                case "8R":
                    pb.Image = Properties.Resources._8R;
                    break;
                case "8H":
                    pb.Image = Properties.Resources._8H;
                    break;
                case "8B":
                    pb.Image = Properties.Resources._8B;
                    break;
                case "9C":
                    pb.Image = Properties.Resources._9C;
                    break;
                case "9R":
                    pb.Image = Properties.Resources._9R;
                    break;
                case "9H":
                    pb.Image = Properties.Resources._9H;
                    break;
                case "9B":
                    pb.Image = Properties.Resources._9B;
                    break;
                case "10C":
                    pb.Image = Properties.Resources._10C;
                    break;
                case "10R":
                    pb.Image = Properties.Resources._10R;
                    break;
                case "10H":
                    pb.Image = Properties.Resources._10H;
                    break;
                case "10B":
                    pb.Image = Properties.Resources._10B;
                    break;
                case "JC":
                    pb.Image = Properties.Resources.JC;
                    break;
                case "JR":
                    pb.Image = Properties.Resources.JR;
                    break;
                case "JH":
                    pb.Image = Properties.Resources.JH;
                    break;
                case "JB":
                    pb.Image = Properties.Resources.JB;
                    break;
                case "QC":
                    pb.Image = Properties.Resources.QC;
                    break;
                case "QR":
                    pb.Image = Properties.Resources.QR;
                    break;
                case "QH":
                    pb.Image = Properties.Resources.QH;
                    break;
                case "QB":
                    pb.Image = Properties.Resources.QB;
                    break;
                case "KC":
                    pb.Image = Properties.Resources.KC;
                    break;
                case "KR":
                    pb.Image = Properties.Resources.KR;
                    break;
                case "KH":
                    pb.Image = Properties.Resources.KH;
                    break;
                case "KB":
                    pb.Image = Properties.Resources.KB;
                    break;
            }
            return pb;
        }

        private void frmChallenger_Load(object sender, EventArgs e)
        {
            textBox2.Text = "Mã phòng: " + roomInfo.RoomID + "\r\nTiền cược tối thiểu: " + roomInfo.Minbet.ToString();
            label1.Text = dealerInfo.playername;
            label2.Text = userInfo.playername;
            button3.Enabled = false;
            button4.Enabled = false;
            timer1.Start();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox4.Text = "";
            if (textBox1.Text.All(Char.IsDigit) == false || textBox1.Text == "")
            {
                MessageBox.Show("Tiền cược tối thiểu phải là số nguyên không âm", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else if (Int32.Parse(textBox1.Text) < MinBet)
            {
                MessageBox.Show("Tiền cược phải cao hơn hoặc bằng tiền cược tối thiểu.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
            }
            else if (Int32.Parse(textBox1.Text) > Int32.Parse(userInfo.Money))
            {
                MessageBox.Show("Số tiền hiện có không đủ để đặt cược", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int cmd = 3;
                int bet = Int32.Parse(textBox1.Text);
                textBox1.Enabled = false;
                betMoney = bet;
                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress IP = IPAddress.Parse(roomInfo.IpAddress);
                IPEndPoint ipEp = new IPEndPoint(IP, roomInfo.Port);
                tcpClient.Connect(ipEp);

                byte[] sendBuffer = new byte[2048];
                DataManager dataManager = new DataManager();

                GameControll data = new GameControll(cmd, bet);
                sendBuffer = dataManager.SerializeData(data);

                tcpClient.Send(sendBuffer);
                tcpClient.Shutdown(SocketShutdown.Send);

                byte[] receiveBuffer = new byte[2048];
                tcpClient.Receive(receiveBuffer);
                tcpClient.Shutdown(SocketShutdown.Receive);

                player = (Player)dataManager.DeserializeData(receiveBuffer);



                Dealer1.Image = Properties.Resources.PP;
                Dealer1.Visible = true;
                Dealer2.Image = Properties.Resources.PP;
                Dealer2.Visible = true;
                Dealer3.Image = null;
                Dealer4.Image = null;
                Dealer5.Image = null;

                List<Card> card = player.GetCards();
                Player1.Image = ShowCard(card[0].GetID()).Image;
                Player1.Visible = true;
                Player2.Image = ShowCard(card[1].GetID()).Image;
                Player2.Visible = true;
                Player3.Image = null;
                Player4.Image = null;
                Player5.Image = null;

                button1.Enabled = false;
                button3.Visible = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }

        }

        private void frmChallenger_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cmd = 2;
            string str = "";
            GameControll data = new GameControll(cmd, str);

            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse(roomInfo.IpAddress);
            IPEndPoint ipEp = new IPEndPoint(IP, roomInfo.Port);
            tcpClient.Connect(ipEp);
          
            byte[] sendBuffer = new byte[2048];
            DataManager dataManager = new DataManager();
            sendBuffer = dataManager.SerializeData(data);

            tcpClient.Send(sendBuffer);
            tcpClient.Shutdown(SocketShutdown.Send);

            this.Hide();
            frmStart frm = new frmStart(userInfo);
            frm.Show();
        }


        

        private void button3_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {


            
        }

        public void DoStuff()
        {
            bool check = false;
            while (check == false)
            {
                int cmd = 5;
                string str = "";
                GameControll data = new GameControll(cmd, str);

                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress IP = IPAddress.Parse(roomInfo.IpAddress);
                IPEndPoint ipEp = new IPEndPoint(IP, roomInfo.Port);
                tcpClient.Connect(ipEp);

                byte[] sendBuffer = new byte[2048];
                DataManager dataManager = new DataManager();
                sendBuffer = dataManager.SerializeData(data);

                tcpClient.Send(sendBuffer);
                tcpClient.Shutdown(SocketShutdown.Send);

                byte[] receiveBuffer = new byte[2048];
                tcpClient.Receive(receiveBuffer);
                tcpClient.Shutdown(SocketShutdown.Receive);

                GameControll result = (GameControll)dataManager.DeserializeData(receiveBuffer);


                GameStatue game = (GameStatue)result.obj;

                dealer = game.dealerHand;
                List<Card> card = dealer.GetCards();

                Thread.Sleep(1000);
                switch (dealer.GetNumberOfCards())
                {
                    case 3:
                        {
                            Dealer3.Image = Properties.Resources.PP;
                            Dealer3.Visible = true;
                            break;
                        }
                    case 4:
                        {
                            Dealer4.Image = Properties.Resources.PP;
                            Dealer4.Visible = true;
                            break;
                        }
                    case 5:
                        {
                            Dealer5.Image = Properties.Resources.PP;
                            Dealer5.Visible = true;
                            break;
                        }
                }

                uWin = result.controll;
                check = game.gameEnd;
            }

            List<Card> card1 = dealer.GetCards();
            switch (dealer.GetNumberOfCards())
            {
                case 2:
                    {
                        Dealer1.Image = ShowCard(card1[0].GetID()).Image;
                        Dealer1.Visible = true;
                        Dealer2.Image = ShowCard(card1[1].GetID()).Image;
                        Dealer2.Visible = true;
                        Dealer3.Image = null;
                        Dealer4.Image = null;
                        Dealer5.Image = null;
                        break;
                    }
                case 3:
                    {
                        Dealer1.Image = ShowCard(card1[0].GetID()).Image;
                        Dealer1.Visible = true;
                        Dealer2.Image = ShowCard(card1[1].GetID()).Image;
                        Dealer2.Visible = true;
                        Dealer3.Image = ShowCard(card1[2].GetID()).Image;
                        Dealer3.Visible = true;
                        Dealer4.Image = null;
                        Dealer5.Image = null;
                        break;
                    }
                case 4:
                    {
                        Dealer1.Image = ShowCard(card1[0].GetID()).Image;
                        Dealer1.Visible = true;
                        Dealer2.Image = ShowCard(card1[1].GetID()).Image;
                        Dealer2.Visible = true;
                        Dealer3.Image = ShowCard(card1[2].GetID()).Image;
                        Dealer3.Visible = true;
                        Dealer4.Image = ShowCard(card1[3].GetID()).Image;
                        Dealer4.Visible = true;
                        Dealer5.Image = null;
                        break;
                    }
                case 5:
                    {
                        Dealer1.Image = ShowCard(card1[0].GetID()).Image;
                        Dealer1.Visible = true;
                        Dealer2.Image = ShowCard(card1[1].GetID()).Image;
                        Dealer2.Visible = true;
                        Dealer3.Image = ShowCard(card1[2].GetID()).Image;
                        Dealer3.Visible = true;
                        Dealer4.Image = ShowCard(card1[3].GetID()).Image;
                        Dealer4.Visible = true;
                        Dealer5.Image = ShowCard(card1[4].GetID()).Image;
                        Dealer5.Visible = true;
                        break;
                    }
            }

            string winner = " ";
            switch (uWin)
            {
                case -1:
                    {
                        winner = GameLose.SetLose(moneyChange, betMoney);
                        break;
                    }
                case 0:
                    {
                        winner = GameFair.SetFair();
                        break;
                    }
                case 1:
                    {
                        winner = GameWin.SetWin(moneyChange, betMoney);
                        break;
                    }
            }

            textBox4.Text = winner;

            SocketData data1 = new SocketData((int)SocketCommand.MoneyChange, moneyChange);

            Socket tcpClient1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP1 = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp1 = new IPEndPoint(IP1, 800);

            try
            {
                tcpClient1.Connect(ipEp1);
            }
            catch
            {

            }

            byte[] sendBuffer1 = new byte[2048];
            DataManager dataManager1 = new DataManager();
            sendBuffer1 = dataManager1.SerializeData(data1);

            tcpClient1.Send(sendBuffer1);
            tcpClient1.Shutdown(SocketShutdown.Send);

            button1.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            if (player.GetStatus() < 2)
            {
                int cmd = 4;
                string str = "";
                GameControll data = new GameControll(cmd, str);

                Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress IP = IPAddress.Parse(roomInfo.IpAddress);
                IPEndPoint ipEp = new IPEndPoint(IP, roomInfo.Port);
                tcpClient.Connect(ipEp);

                byte[] sendBuffer = new byte[2048];
                DataManager dataManager = new DataManager();
                sendBuffer = dataManager.SerializeData(data);

                tcpClient.Send(sendBuffer);
                tcpClient.Shutdown(SocketShutdown.Send);

                byte[] receiveBuffer = new byte[2048];
                tcpClient.Receive(receiveBuffer);
                tcpClient.Shutdown(SocketShutdown.Receive);

                player = (Player)dataManager.DeserializeData(receiveBuffer);

                List<Card> card = player.GetCards();
                switch (player.GetNumberOfCards())
                {
                    case 3:
                        {
                            Player3.Image = ShowCard(card[2].GetID()).Image;
                            Player3.Visible = true;
                            break;
                        }
                    case 4:
                        {
                            Player4.Image = ShowCard(card[3].GetID()).Image;
                            Player4.Visible = true;
                            break;
                        }
                    case 5:
                        {
                            Player5.Image = ShowCard(card[4].GetID()).Image;
                            Player5.Visible = true;
                            break;
                        }
                }

                if (player.GetNumberOfCards() == 5)
                {
                    button3.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Điểm của bạn đã lớn hơn 21!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button4.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            Thread thread1 = new Thread(DoStuff);
            thread1.IsBackground = false;
            thread1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int currenmoney = Int32.Parse(moneyChange.CurrentMoney);
            textBox3.Text = $"{currenmoney.ToString("#,##0")} $";
        }

        private void frmChallenger_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager dataManager = new DataManager();
            dataManager.LogOut(userInfo.username);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

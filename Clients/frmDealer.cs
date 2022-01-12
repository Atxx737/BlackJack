using Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clients.Lib;
using Clients.Game;

namespace Clients
{
    public partial class frmDealer : Form
    {

        public UserInfo dealerInfo { get; set; }
        public UserInfo playerInfo;
        public RoomInfo roomInfo { get; set; }
        bool Available = true;
        Player dealer;
        Player player;
        CardSet cardSet;
        int betMoney;
        MoneyChange moneyChange;
        bool gameEnd = false;
        int loser = 0;

        public frmDealer(UserInfo dealer, RoomInfo room)
        {
            dealerInfo = dealer;
            roomInfo = room;
            MoneyChange term = new MoneyChange(dealerInfo.ID, dealerInfo.Money);
            moneyChange = term;
            InitializeComponent();
        }

        private void frmDealer_Load(object sender, EventArgs e)
        {
            label2.Text = dealerInfo.playername;
            textBox1.Text = "Mã phòng: " + roomInfo.RoomID + "\r\nTiền cược tối thiểu: " + roomInfo.Minbet.ToString();
            timer1.Start();
            button2.Enabled = false;
            button3.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.IsBackground = false;
            serverThread.Start();
        }

        void StartUnsafeThread()
        {
            byte[] receiveBuffer = new byte[2048];
            Socket clientSocket;
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipEp = new IPEndPoint(IPAddress.Parse(roomInfo.IpAddress), roomInfo.Port);
            listenerSocket.Bind(ipEp);
            listenerSocket.Listen(10);

            while (true)
            {
                try
                {
                    clientSocket = listenerSocket.Accept();
                    Thread clientThread = new Thread(() =>
                    {
                        DataManager dataManager = new DataManager();
                        clientSocket.Receive(receiveBuffer);
                        GameControll data = (GameControll) dataManager.DeserializeData(receiveBuffer);

                        object result = ProccessData(data);

                        byte[] sendBuffer = new byte[2024];                      
                        sendBuffer = dataManager.SerializeData(result);

                        clientSocket.Send(sendBuffer);
                        clientSocket.Shutdown(SocketShutdown.Send);

                        clientSocket.Close();
                    });
                    clientThread.IsBackground = false;
                    clientThread.Start();

                }
                catch
                {

                }
            }
        }

        //Xử lý yêu cầu người chơi
        public object ProccessData(GameControll data)
        {
            switch (data.controll)
            {
                //Người chơi muốn vào phòng
                case 1:
                    {
                        if(Available == true)
                        {
                            playerInfo = (UserInfo)data.obj;
                            Available = false;
                            label3.Text = playerInfo.playername;
                            label4.Text = "Đang chuẩn bị...";
                            GameControll result = new GameControll(1, dealerInfo);
                            return result;
                        }
                        else
                        {
                            GameControll result = new GameControll(0, "");
                            return 0;
                        }
                        break;
                    }
                //Người chơi muốn rời phòng
                case 2:
                    {
                        Player1.Visible = false;
                        Player2.Visible = false;
                        Player3.Visible = false;
                        Player4.Visible = false;
                        Player5.Visible = false;
                        Dealer1.Visible = false;
                        Dealer2.Visible = false;
                        Dealer3.Visible = false;
                        Dealer4.Visible = false;
                        Dealer5.Visible = false;
                        label3.Text = "Đang chờ đối thủ...";
                        label4.Text = null;
                        textBox3.Text = "";
                        button2.Enabled = false;
                        button3.Enabled = false;
                        Available = true;
                        break;
                    }
                //Người chơi muốn bắt đầu game
                case 3:
                    {
                        betMoney = (int) data.obj;
                        label4.Text = "Đã cược: " + betMoney.ToString();
                        textBox3.Text = "";
                        button2.Enabled = false;
                        button3.Enabled = false;
                        gameEnd = false;
                        loser = 0;
                        Newgame();
                        Player result = player;
                        return result;
                        break;
                    }
                //Người chơi muốn rút bài
                case 4:
                    {
                        DrawCard(player.WithdrawCard(cardSet), player.GetT(), player.GetNumberOfCards(), '1');
                        Player result = player;
                        return result;
                        break;
                    }
                //Người chơi ngưng rút
                case 5:
                    {

                        GameStatue gs = new GameStatue();
                        gs.gameEnd = gameEnd;
                        gs.dealerHand = dealer;

                        GameControll result = new GameControll(loser, gs);
                        if(gameEnd == false)
                        {
                            button2.Enabled = true;
                            button3.Enabled = true;
                        }
                        

                        return result;
                        break;
                    }
            }
            return -1;
        }

        public void Newgame()
        {
           
            Player1.Visible = false;
            Player2.Visible = false;
            Player3.Visible = false;
            Player4.Visible = false;
            Player5.Visible = false;
            Dealer1.Visible = false;
            Dealer2.Visible = false;
            Dealer3.Visible = false;
            Dealer4.Visible = false;
            Dealer5.Visible = false;
            dealer = new Player();
            dealer.SetType('0');
            player = new Player();
            player.SetType('1');
            cardSet = new CardSet();
            DrawCard(dealer.WithdrawCard(cardSet), dealer.GetT(), dealer.GetNumberOfCards(), '0');
            DrawCard(player.WithdrawCard(cardSet), player.GetT(), player.GetNumberOfCards(), '1');
            DrawCard(dealer.WithdrawCard(cardSet), dealer.GetT(), dealer.GetNumberOfCards(), '0');
            DrawCard(player.WithdrawCard(cardSet), player.GetT(), player.GetNumberOfCards(), '1');
            //btnStart.Visible = false;
            //btnRutBai.Visible = true;
            /*if (player.GetStatus() == 0)
            {
                btnGioBai.Visible = true;
            }*/
        }

        public void DrawCard(Card card, char TypeOfPlayer, int NumberOfCard, char Status)
        {
            PictureBox pb = new PictureBox();
            pb.Visible = false;
            pb.Width = 71;
            pb.Height = 96;
            if (Status == '0')
            {
                switch (card.GetID())
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
            }
            else
            {
                pb.Image = Properties.Resources.PP;
            }
            if (TypeOfPlayer == '0')
            {
                switch (NumberOfCard)
                {
                    case 1:
                        Dealer1.Image = pb.Image;
                        Dealer1.Visible = true;
                        break;
                    case 2:
                        Dealer2.Image = pb.Image;
                        Dealer2.Visible = true;
                        break;
                    case 3:
                        Dealer3.Image = pb.Image;
                        Dealer3.Visible = true;
                        break;
                    case 4:
                        Dealer4.Image = pb.Image;
                        Dealer4.Visible = true;
                        break;
                    case 5:
                        Dealer5.Image = pb.Image;
                        Dealer5.Visible = true;
                        break;
                }
            }
            else
            {
                switch (NumberOfCard)
                {
                    case 1:
                        Player1.Image = pb.Image;
                        Player1.Visible = true;
                        break;
                    case 2:
                        Player2.Image = pb.Image;
                        Player2.Visible = true;
                        break;
                    case 3:
                        Player3.Image = pb.Image;
                        Player3.Visible = true;
                        break;
                    case 4:
                        Player4.Image = pb.Image;
                        Player4.Visible = true;
                        break;
                    case 5:
                        Player5.Image = pb.Image;
                        Player5.Visible = true;
                        break;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmDealer_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager dataManager = new DataManager();
            dataManager.LogOut(dealerInfo.username);
            CloseRoom();
        }

        public  void CloseRoom()
        {
            
            SocketData data = new SocketData((int)SocketCommand.CloseRoom, roomInfo);

            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress IP = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEp = new IPEndPoint(IP, 800);
            Thread.Sleep(1000);
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

            byte[] reseiveBuffer = new byte[2048];
            tcpClient.Receive(reseiveBuffer);
            tcpClient.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseRoom();
            this.Hide();
            frmStart frm = new frmStart(dealerInfo);
            frm.Show();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Dealer1_Click(object sender, EventArgs e)
        {

        }

        private void Dealer4_Click(object sender, EventArgs e)
        {

        }

        private void Dealer3_Click(object sender, EventArgs e)
        {

        }

        private void Player3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (dealer.GetStatus() < 2)
            {
                DrawCard(dealer.WithdrawCard(cardSet), dealer.GetT(), dealer.GetNumberOfCards(), '0');
                if (dealer.GetStatus() >= 0)
                {
                    button3.Visible = true;
                }
                if (dealer.GetNumberOfCards() == 5)
                {
                    button2.Visible = false;
                }

            }
            else
            {
                MessageBox.Show("Điểm của bạn đã lớn hơn 21!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            List<Card> card = player.GetCards();
            switch (player.GetNumberOfCards())
            {
                case 2:
                    {
                        Player1.Image = ShowCard(card[0].GetID()).Image;
                        Player1.Visible = true;
                        Player2.Image = ShowCard(card[1].GetID()).Image;
                        Player2.Visible = true;
                        Player3.Image = null;
                        Player4.Image = null;
                        Player5.Image = null;
                        break;
                    }
                case 3:
                    {
                        Player1.Image = ShowCard(card[0].GetID()).Image;
                        Player1.Visible = true;
                        Player2.Image = ShowCard(card[1].GetID()).Image;
                        Player2.Visible = true;
                        Player3.Image = ShowCard(card[2].GetID()).Image;
                        Player3.Visible = true;
                        Player4.Image = null;
                        Player5.Image = null;
                        break;
                    }
                case 4:
                    {
                        Player1.Image = ShowCard(card[0].GetID()).Image;
                        Player1.Visible = true;
                        Player2.Image = ShowCard(card[1].GetID()).Image;
                        Player2.Visible = true;
                        Player3.Image = ShowCard(card[2].GetID()).Image;
                        Player3.Visible = true;
                        Player4.Image = ShowCard(card[3].GetID()).Image;
                        Player4.Visible = true;
                        break;
                    }
                case 5:
                    {
                        Player1.Image = ShowCard(card[0].GetID()).Image;
                        Player1.Visible = true;
                        Player2.Image = ShowCard(card[1].GetID()).Image;
                        Player2.Visible = true;
                        Player3.Image = ShowCard(card[2].GetID()).Image;
                        Player3.Visible = true;
                        Player4.Image = ShowCard(card[3].GetID()).Image;
                        Player4.Visible = true;
                        Player5.Image = ShowCard(card[4].GetID()).Image;
                        Player5.Visible = true;
                        break;
                    }
            }

            string winner = " ";
            char ch = '0';//Check if speacial case
            if (dealer.GetNumberOfCards() == 5 && player.GetNumberOfCards() != 5)
            {
                if (dealer.GetStatus() == 0)
                {
                    winner = GameWin.SetWin(moneyChange, betMoney);
                    loser = -1;
                    ch = '1';
                }
            }
            else
            {
                if (dealer.GetNumberOfCards() != 5 && player.GetNumberOfCards() == 5)
                {
                    if (player.GetStatus() == 0)
                    {
                        winner = GameLose.SetLose(moneyChange, betMoney);
                        loser = 1;
                        ch = '1';
                    }
                }
            }
            if (ch == '0')
            {
                if (dealer.GetStatus() != 0 && player.GetStatus() == 0)
                {
                    winner = GameLose.SetLose(moneyChange, betMoney);
                    loser = 1;
                }
                else
                {
                    if (dealer.GetStatus() == 0 && player.GetStatus() != 0)
                    {
                        winner = GameWin.SetWin(moneyChange, betMoney);
                        loser = -1;
                    }
                    else
                    {
                        if (dealer.GetStatus() != 0 && player.GetStatus() != 0)
                        {
                            winner = GameFair.SetFair();
                        }
                        else
                        {
                            if (dealer.GetMark() > player.GetMark())
                            {
                                winner = GameWin.SetWin(moneyChange, betMoney);
                                loser = -1;
                            }
                            else
                            {
                                if (dealer.GetMark() == player.GetMark())
                                {
                                    winner = GameFair.SetFair();
                                }
                                else
                                {
                                    winner = GameLose.SetLose(moneyChange, betMoney);
                                    loser = 1;
                                }
                            }
                        }
                    }
                }
            }

            textBox3.Text = winner;

            button2.Enabled = false;
            button3.Enabled = false;

            SocketData data = new SocketData((int)SocketCommand.MoneyChange, moneyChange);

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
            gameEnd = true;

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int currenmoney = Int32.Parse(moneyChange.CurrentMoney);
            textBox2.Text = $"{currenmoney.ToString("#,##0")} $";
        }

        private void frmDealer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

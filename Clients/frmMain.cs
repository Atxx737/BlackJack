using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Clients.Game;
using Lib;
using System.Net.Sockets;
using System.Net;
using Clients.Lib;

namespace Clients
{
    public partial class frmMain : Form
    {
        CardSet cardSet;
        Player com;
        Player user;
        UserInfo userInfo;
        int betMoney;
        MoneyChange moneyChange;
        public frmMain(UserInfo data)
        {
            userInfo = data;
            MoneyChange term = new MoneyChange(userInfo.ID, userInfo.Money);
            moneyChange = term;
            InitializeComponent();
        }

        #region Click Anywhere to Move
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        private void MoveForm(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        #endregion

        private void TxtEnter(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                btnBet_Click(null, null);
            }
        }
        

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
                Application.Exit();
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            string playername = userInfo.playername;
            
            sttWelcome.Text = $"Xin chào {playername}";

            MainTimer.Start();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            sttTime.Text = DateTime.Now.ToString("HH:mm:ss"); //hiển thị giờ

            int currenmoney = Int32.Parse(moneyChange.CurrentMoney);
            lblCurMoney.Text = $"{currenmoney.ToString("#,##0")} $";
            
        }
        

        public void Newgame()
        {
            GrResult.Visible = false;
            Player1.Visible = false;
            Player2.Visible = false;
            Player3.Visible = false;
            Player4.Visible = false;
            Player5.Visible = false;
            Cpu1.Visible = false;
            Cpu2.Visible = false;
            Cpu3.Visible = false;
            Cpu4.Visible = false;
            Cpu5.Visible = false;
            com = new Player();
            com.SetType('0');
            user = new Player();
            user.SetType('1');
            cardSet = new CardSet();
            DrawCard(com.WithdrawCard(cardSet), com.GetT(), com.GetNumberOfCards(), '0');
            DrawCard(user.WithdrawCard(cardSet), user.GetT(), user.GetNumberOfCards(), '1');
            DrawCard(com.WithdrawCard(cardSet), com.GetT(), com.GetNumberOfCards(), '0');
            DrawCard(user.WithdrawCard(cardSet), user.GetT(), user.GetNumberOfCards(), '1');
            btnStart.Visible = false;
            btnRutBai.Visible = true;
            if (user.GetStatus() == 0)
            {
                btnGioBai.Visible = true;
            }
        }
        public void DrawCard(Card card, char TypeOfPlayer, int NumberOfCard, char Status)
        {
            PictureBox pb = new PictureBox();
            pb.Visible = false;
            pb.Width = 71;
            pb.Height = 96;
            if (Status == '1')
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
                        Cpu1.Image = pb.Image;
                        Cpu1.Visible = true;
                        break;
                    case 2:
                        Cpu2.Image = pb.Image;
                        Cpu2.Visible = true;
                        break;
                    case 3:
                        Cpu3.Image = pb.Image;
                        Cpu3.Visible = true;
                        break;
                    case 4:
                        Cpu4.Image = pb.Image;
                        Cpu4.Visible = true;
                        break;
                    case 5:
                        Cpu5.Image = pb.Image;
                        Cpu5.Visible = true;
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (betMoney == 0)
            {
                MessageBox.Show("Bạn chưa đặt cược, không thể chơi","Cảnh báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtBetMoney.Focus();
            }
            else { Newgame(); btnRutBai.Focus(); }
        }

        private void btnRutBai_Click(object sender, EventArgs e)
        {
            if (user.GetStatus() < 2)
            {
                DrawCard(user.WithdrawCard(cardSet), user.GetT(), user.GetNumberOfCards(), '1');
                if (user.GetStatus() >= 0)
                {
                    btnGioBai.Visible = true;
                }
                if (user.GetStatus() > 0 || user.GetNumberOfCards() == 5)
                {
                    btnRutBai.Visible = false;
                }

            }
            else
            {
                MessageBox.Show("Điểm của bạn đã lớn hơn 21!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGioBai_Click(object sender, EventArgs e)
        {
            string winner = " ";
            if (com.GetStatus() < 0)
            {
                while (com.GetStatus() < 0 && com.GetNumberOfCards() < 5)
                {
                    this.DrawCard(com.WithdrawCard(cardSet), com.GetT(), com.GetNumberOfCards(), '0');
                }
            }

            char ch = '0';//Check if speacial case
            if (com.GetNumberOfCards() == 5 && user.GetNumberOfCards() != 5)
            {
                if (com.GetStatus() == 0)
                {
                    winner = GameLose.SetLose(moneyChange, betMoney);
                    ch = '1';
                }
            }
            else
            {
                if (com.GetNumberOfCards() != 5 && user.GetNumberOfCards() == 5)
                {
                    if (user.GetStatus() == 0)
                    {
                        winner = GameWin.SetWin(moneyChange, betMoney);
                        ch = '1';
                    }
                }
            }
            if (ch == '0')
            {
                if (com.GetStatus() != 0 && user.GetStatus() == 0)
                {
                    winner = GameWin.SetWin(moneyChange, betMoney);
                }
                else
                {
                    if (com.GetStatus() == 0 && user.GetStatus() != 0)
                    {
                        winner = GameLose.SetLose(moneyChange, betMoney);
                    }
                    else
                    {
                        if (com.GetStatus() != 0 && user.GetStatus() != 0)
                        {
                            winner = GameFair.SetFair();
                        }
                        else
                        {
                            if (com.GetMark() > user.GetMark())
                            {
                                winner = GameLose.SetLose(moneyChange, betMoney);
                            }
                            else
                            {
                                if (com.GetMark() == user.GetMark())
                                {
                                    winner = GameFair.SetFair();
                                }
                                else
                                {
                                    winner = GameWin.SetWin(moneyChange, betMoney);
                                }
                            }
                        }
                    }
                }
            }

            List<Card> comCards = com.GetCards();
            for (int k = 0; k < comCards.Count; k++)
            {
                this.DrawCard(comCards[k], com.GetT(), k + 1, '1');
            }



            GrResult.Visible = true;
            lblWinner.Text = winner;

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

            txtBetMoney.Enabled = true;

            btnStart.Visible = true;
            btnRutBai.Visible = false;
            btnGioBai.Visible = false;
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            int betmoney;
            if (int.TryParse(txtBetMoney.Text, out betmoney))
            {
                if (betmoney > Int32.Parse(userInfo.Money))
                {
                    MessageBox.Show("Số tiền hiện có không đủ để đặt cược", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (betmoney == 0)
                {
                    MessageBox.Show($"Bạn đã hết tiền", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    txtBetMoney.Text = betmoney.ToString("#,##0");
                    betMoney = betmoney;
                    txtBetMoney.Enabled = false;
                    btnStart.Focus();
                } 
            }
            else
            {
                MessageBox.Show($"Số tiền nhập vào không hợp lệ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBetMoney.Clear();
            }


        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            /*frmInfo frm = new frmInfo();
            frm.ShowDialog();
            frm.Focus();*/
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            /*MainTimer.Stop();
            Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
            frm.Focus();*/
        }

        private void txtPlayerPoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void Player3_Click(object sender, EventArgs e)
        {

        }

        private void txtTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblGold_Click(object sender, EventArgs e)
        {

        }

        private void MenuStr_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }



        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void StatusStr_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Bạn có thực sự muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1) == DialogResult.Yes) == true)
            {
                MainTimer.Stop();
                Hide();
                frmStart frm = new frmStart(userInfo);
                frm.Show();
                frm.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmInfo frm = new frmInfo(userInfo);
            frm.Show();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataManager dataManager = new DataManager();
            dataManager.LogOut(userInfo.username);
        }
    }
}
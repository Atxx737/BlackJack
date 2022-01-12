using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.Lib;
using Lib;
using System.Data.SqlClient;

namespace Server
{

    public partial class Form1 : Form
    {

        public List<LoginInfo> logList = new List<LoginInfo>();  

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.IsBackground = false;
            serverThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        void StartUnsafeThread()
        {
            byte[] receiveBuffer = new byte[2048]; 
            
            Socket clientSocket;
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 800);
            listenerSocket.Bind(ipEp);
            listenerSocket.Listen(10);

            while(true)
            {
                try
                {
                    clientSocket = listenerSocket.Accept();
                    Thread clientThread = new Thread(() =>
                    {
                        clientSocket.Receive(receiveBuffer);
                        SocketData data = (SocketData) DeserializeData(receiveBuffer);

                        object result = ProccessData(data);
                       
                        byte[] sendBuffer = new byte[2024];
                        sendBuffer = SerializeData(result);

                        clientSocket.Send(sendBuffer);
                        clientSocket.Shutdown(SocketShutdown.Send);                  

                        clientSocket.Close();
                    });
                    clientThread.IsBackground = false;
                    clientThread.Start();
                    
                }
                catch  (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Nén đối tượng thành mảng byte[]
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }

        /// <summary>
        /// Giải nén mảng byte[] thành đối tượng object
        /// </summary>
        /// <param name="theByteArray"></param>
        /// <returns></returns>
        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return bf1.Deserialize(ms);
        }

        private  object ProccessData(SocketData data)
        {
            switch (data.command)
            {
                case (int)SocketCommand.Register:
                    {
                        RegisterInfo term = (RegisterInfo)data.obJect;
                        ServerInfo.OpenConnect();
                        if (ExistingUsername.Check(term.username) == true) //check tên tài khoản
                        {
                            return "re";
                        }
                        else
                        {
                            if (Register.Do(term) == true) //đăng kí
                            {
                                return "rs";
                            }
                            else
                            {
                                MessageBox.Show("Đã có lỗi xảy ra. Đăng kí thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    }
                case (int)SocketCommand.Login:
                    {
                        LoginInfo term = (LoginInfo)data.obJect;

                        for (int i = 0; i < logList.Count(); i++)
                        {
                            if (term.username == logList[i].username)
                            {
                                SocketData result = new SocketData(2, "");
                                return result;
                            }
                        }

                        ServerInfo.OpenConnect();
                        if (Login.Do(term) == true)
                        {
                            UserInfo userInfo = new UserInfo();
                            string query = $"SELECT * FROM dbo.Player WHERE Username = '{term.username}'";
                            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
                            SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                            if (sqlreader.Read())
                            {
                                userInfo.ID = sqlreader["ID"].ToString();
                                userInfo.username = sqlreader["Username"].ToString();
                                userInfo.playername = sqlreader["Playername"].ToString();
                                userInfo.Money = sqlreader["Money"].ToString();
                                userInfo.VIP = sqlreader["Playername"].ToString();
                            }
                            logList.Add(term);
                            sqlreader.Close();
                            SocketData result = new SocketData(1, userInfo);
                            return result;
                        }
                        else
                        {
                            SocketData result = new SocketData(0, "");
                            return result;
                        }
                    }
                case (int)SocketCommand.MoneyChange:
                    {

                        MoneyChange moneyChange = (MoneyChange)data.obJect;
                        ServerInfo.OpenConnect();
                        SqlCommand sqlcmd = new SqlCommand($"Update dbo.Player SET Money='{moneyChange.CurrentMoney}' where ID = '{moneyChange.ID}'", ServerInfo.connection);
                        SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                        sqlreader.Close();
                        break;
                    }
                case (int)SocketCommand.RankDetail:
                    {
                        List<UserInfo> ILIst = new List<UserInfo>();
                        ServerInfo.OpenConnect();
                        SqlCommand sqlcmd = new SqlCommand($"Select ID, Playername, Money from dbo.Player Where Money = (Select MAX(Money) From dbo.Player)", ServerInfo.connection);
                        SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            UserInfo userInfo = new UserInfo();
                            userInfo.ID = sqlreader["ID"].ToString();
                            userInfo.playername = sqlreader["Playername"].ToString();
                            userInfo.Money = sqlreader["Money"].ToString();
                            ILIst.Add(userInfo);
                        }
                        sqlreader.Close();

                        SqlCommand sqlcmd1 = new SqlCommand($"Select ID, Playername, Money from dbo.Player Where Money = (SELECT MAX(Money) FROM dbo.Player WHERE Money < (SELECT MAX(Money) FROM dbo.Player) )", ServerInfo.connection);
                        SqlDataReader sqlreader1 = sqlcmd1.ExecuteReader();
                        if (sqlreader1.Read())
                        {
                            UserInfo userInfo = new UserInfo();
                            userInfo.ID = sqlreader1["ID"].ToString();
                            userInfo.playername = sqlreader1["Playername"].ToString();
                            userInfo.Money = sqlreader1["Money"].ToString();
                            ILIst.Add(userInfo);
                        }
                        sqlreader1.Close();

                        SqlCommand sqlcmd2 = new SqlCommand($"Select ID, Playername, Money From dbo.Player where Money = (select MAX(Money) from dbo.Player Where Money < (Select MAX(Money) FROM dbo.Player WHERE Money < (SELECT MAX(Money) FROM dbo.Player)))", ServerInfo.connection);
                        SqlDataReader sqlreader2 = sqlcmd2.ExecuteReader();
                        if (sqlreader2.Read())
                        {
                            UserInfo userInfo = new UserInfo();
                            userInfo.ID = sqlreader2["ID"].ToString();
                            userInfo.playername = sqlreader2["Playername"].ToString();
                            userInfo.Money = sqlreader2["Money"].ToString();
                            ILIst.Add(userInfo);
                        }
                        sqlreader2.Close();

                        return ILIst;

                    }
                case (int)SocketCommand.CreateRoom:
                    {
                        RoomInfo term = (RoomInfo)data.obJect;
                        ServerInfo.OpenConnect();
                        if (ExistingRoomID.Check(term.RoomID) == true) //check mã phòng
                        {
                            return "re";
                        }
                        else
                        {
                            if (CreateRoom.Do(term) == true) //tạo phòng
                            {
                                return "rs";
                            }
                            else
                            {
                                MessageBox.Show("Đã có lỗi xảy ra. Đăng kí thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    }
                case (int)SocketCommand.Join:
                    {
                        string roomID = (string)data.obJect;
                        ServerInfo.OpenConnect();
                        if (JoinRoom.Do(roomID) == true)
                        {
                            string IP = "";
                            string ID = "";
                            string Port = "";
                            string MinBet = "";

                            string query = $"Select RoomID, MinBet, IPAddress, Port from dbo.Room where RoomID = '{roomID}'";
                            SqlCommand sqlcmd = new SqlCommand(query, ServerInfo.connection);
                            SqlDataReader sqlreader = sqlcmd.ExecuteReader();

                            if (sqlreader.Read())
                            {
                                IP = sqlreader["IPAddress"].ToString();
                                MinBet = sqlreader["Minbet"].ToString();
                                Port = sqlreader["Port"].ToString();
                                ID = sqlreader["RoomID"].ToString();
                            }
                            sqlreader.Close();

                            RoomInfo roomInfo = new RoomInfo(IP, Int32.Parse(Port), ID, Int32.Parse(MinBet));
                            SocketData result = new SocketData(1, roomInfo);
                            return result;
                        }
                        else
                        {
                            SocketData result = new SocketData(0, "");
                            return result;
                        }
                        break;
                        } 
                case (int)SocketCommand.CloseRoom:
                    {
                        RoomInfo term = (RoomInfo) data.obJect;
                        ServerInfo.OpenConnect();
                        SqlCommand sqlcmd = new SqlCommand($"delete from dbo.Room where RoomID = '{term.RoomID}'", ServerInfo.connection);
                        SqlDataReader sqlreader = sqlcmd.ExecuteReader();
                        break;
                    }
                case (int)SocketCommand.LogOut:
                    {
                        string term = (string)data.obJect;
                        for (int i = 0; i < logList.Count(); i++)
                        {
                            if(logList[i].username == term)
                            {
                                logList.Remove(logList[i]);
                            }
                        }
                        break;
                    }
            }
            return -1;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [Serializable]
    public class SocketData
    {
        public int command;
        public int Command
        {
            get { return command; }
            set { command = value; }
        }

        public Object obJect { get; set; }


        public SocketData(int command, Object obJect)
        {
            this.Command = command;
            this.obJect = obJect;
        }
    }

    [Serializable]
    public class UserInfo
    {
        public string ID { get; set; }
        public string username { get; set; }
        public string playername { get; set; }
        public string Money { get; set; }
        public string VIP { get; set; }
    }

    [Serializable]
    public class RegisterInfo
    {
        public string playername { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    [Serializable]
    public class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }

        public LoginInfo (string us, string pass)
        {
            username = us;
            password = pass;
        }
    }

    [Serializable]
    public class MoneyChange
    {
        public string ID { get; set; }
        public string CurrentMoney { get; set; }
        public MoneyChange (string id, string cm)
        {
            ID = id;
            CurrentMoney = cm;
        }
    }

    [Serializable]
    public class RoomInfo
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string RoomID { get; set; }
        public int Minbet { get; set; }
        public RoomInfo(string ip, int port, string id, int min)
        {
            IpAddress = ip;
            Port = port;
            RoomID = id;
            Minbet = min;
        }
    }

 
    [Serializable]
    public enum SocketCommand
    {
        Register,
        Login,
        MoneyChange,
        RankDetail,
        CreateRoom,
        Join,
        CloseRoom,
        LogOut
    }
}

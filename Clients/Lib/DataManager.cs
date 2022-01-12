using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;

namespace Clients.Lib
{
    public class DataManager
    {

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

        public void LogOut(string str)
        {
            SocketData data = new SocketData((int)SocketCommand.LogOut, str);

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

            byte[] reseiveBuffer = new byte[2048];
            tcpClient.Receive(reseiveBuffer);
        }
    }

}

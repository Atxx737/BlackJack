using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clients.Game
{
    [Serializable]
    public class GameControll
    {
        public int controll;
        public object obj;

        public GameControll(int a, object b)
        {
            controll = a;
            obj = b;
        }
    }
}

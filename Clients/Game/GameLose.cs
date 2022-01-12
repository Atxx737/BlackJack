using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;

namespace Clients.Game
{
    public class GameLose
    {
        public static string SetLose(MoneyChange user, int bet)
        {
            int result = Int32.Parse(user.CurrentMoney) - bet;
            user.CurrentMoney = result.ToString();

            return "Bạn là người thua cuộc";
        }
    }
}

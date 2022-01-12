using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Lib;

namespace Clients.Game
{
    public class GameWin
    {
        
        public static string SetWin(MoneyChange user, int bet)
        {
            int result = Int32.Parse(user.CurrentMoney) + bet;
            user.CurrentMoney = result.ToString();

            return "Bạn là người chiến thắng";
        }
    }
}

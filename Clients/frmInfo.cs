using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;

namespace Clients
{
    public partial class frmInfo : Form
    {
        UserInfo userInfo;
        public frmInfo(UserInfo us)
        {
            userInfo = us;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = userInfo.playername;
            label2.Text = userInfo.ID;
            textBox2.Text = userInfo.Money;
            label4.Text = "VIP0";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

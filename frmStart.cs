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
    public partial class frmStart : Form
    {
        UserInfo userInfo;
        public frmStart(UserInfo data)
        {
            userInfo = data;
            InitializeComponent();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain frm = new frmMain(userInfo);
            frm.Show();
        }
    }
}

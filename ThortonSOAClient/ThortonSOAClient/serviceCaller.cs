using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThortonSOAClient
{
    public partial class serviceCaller : Form
    {
        public serviceCaller()
        {
            InitializeComponent();
        }

        private void serviceCaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void param1Lbl_Click(object sender, EventArgs e)
        {

        }

        private void teamNametb_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

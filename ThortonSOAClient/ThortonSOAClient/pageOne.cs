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
    public partial class pageOne : Form
    {
        public pageOne()
        {
            InitializeComponent();
        }

        private void pageOne_Load(object sender, EventArgs e)
        {
            serviceSelectCB.Items.Add("Service 1");
        }

        private void executeBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(serviceSelectCB.Text))
            {
                error1.SetError(serviceSelectCB,"You must select a service before you can start.");
            }
            else
            { 
                this.Hide();
                serviceCaller sc = new serviceCaller();
                sc.Show();
            }
        }
    }
}

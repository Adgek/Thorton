using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThortonSOAClient.UI_Elements;

namespace ThortonSOAClient
{
    public partial class serviceCallerForm : Form
    {
        private static List<ArgumentArea> argumentElements = new List<ArgumentArea>();

        private string ServiceInformation = "";

        private int numArgs = 5;

        private static int yLocation = 10;


        public serviceCallerForm()
        {
            InitializeComponent();
        }

        public serviceCallerForm(string message)
        {
            InitializeComponent();
            ServiceInformation = message;
        }

        private void serviceCaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void serviceCallerForm_Load(object sender, EventArgs e)
        {
            CreateInputArea();
        }

        private void CreateInputArea()
        {
            ArgumentArea a;
            for (int x = 0; x < numArgs; x++)
            {
                a = new ArgumentArea("Argument" + x.ToString());
                argumentElements.Add(a);
                a.Lbl.Location = new Point(0, yLocation);
                a.Tb.Location = new Point(0, yLocation + 25);
                argPanel.Controls.Add(a.Tb);
                argPanel.Controls.Add(a.Lbl);
                yLocation += 65;
            }
        }
    }
}

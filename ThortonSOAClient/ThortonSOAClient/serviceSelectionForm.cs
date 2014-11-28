using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketClass;
using HL7Records;
using System.Configuration;


namespace ThortonSOAClient
{
    public partial class serviceSelectionForm : Form
    {
        private static string TeamName = ConfigurationManager.AppSettings["teamName"];

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static HL7Handler handler = new HL7Handler();
        private static Service service = new Service();

        private static string[] methods = {"GIORP-TOTAL"};

        public serviceSelectionForm()
        {
            InitializeComponent();
        }

        private void pageOne_Load(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "The program has started!");
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
                serviceCallerForm sc = new serviceCallerForm();
                sc.Show();
                //string method = methods[serviceSelectCB.SelectedIndex];
                //string command = "";
                //string returnMsg = SocketSender.StartClient(command);
                //if(returnMsg.Contains("SOA NOT OK"))
                //{

                //}
                //else
                //{ 
                    
                //}
            }

        }
    }
}

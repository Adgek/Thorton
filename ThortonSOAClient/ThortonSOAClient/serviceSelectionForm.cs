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
using System.Configuration;
using HL7Lib.HL7;
using HL7Lib.ServiceData;


namespace ThortonSOAClient
{
    public partial class serviceSelectionForm : Form
    {
        private static string TeamName = ConfigurationManager.AppSettings["teamName"];

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static HL7Handler handler = new HL7Handler();
        private static Service service;

        private static string registryIP = ConfigurationManager.AppSettings["registryipaddress"];
        private static int registryPort = Convert.ToInt32(ConfigurationManager.AppSettings["registryport"]);

        private static string[] methods = ConfigurationManager.AppSettings["tagNames"].Split('|');
        private static string[] serviceNames = ConfigurationManager.AppSettings["serviceNames"].Split('|');

        public serviceSelectionForm()
        {
            InitializeComponent();
        }

        private void pageOne_Load(object sender, EventArgs e)
        {
            foreach (string name in serviceNames)
            {
                serviceSelectCB.Items.Add(name);
            }
            logger.Log(LogLevel.Info, "The program has started!");
            
        }

        private void executeBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(serviceSelectCB.Text))
            {
                error1.SetError(serviceSelectCB,"You must select a service before you can start.");
            }
            else
            {
                service = new Service();
                service.TeamName = TeamName;
                HL7 returnMsg;
                try
                {
                    returnMsg = handler.HandleResponse(SocketSender.StartClient(handler.RegisterTeamMessage(service), registryIP, registryPort));
                   
                }
                catch(Exception ex)
                {
                    throw new NotImplementedException(ex.Message);
                }
                List<string> fields = returnMsg.segments.FirstOrDefault().fields;
                if (fields[1] == "OK")
                {                    
                    service.TeamID = fields[2];
                    service.Tag = methods[serviceSelectCB.SelectedIndex];
                    returnMsg = handler.HandleResponse(SocketSender.StartClient(handler.QueryServiceMessage(service), registryIP, registryPort));
                    fields = returnMsg.segments.FirstOrDefault().fields;

                    if (fields[1] == "OK")
                    {
                        this.Hide();
                        serviceCallerForm sc = new serviceCallerForm(returnMsg, service.TeamName, service.TeamID, this);
                        sc.Show();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    } 
                }
                else
                {
                    throw new NotImplementedException();
                }                
            }            
        }
    }
}

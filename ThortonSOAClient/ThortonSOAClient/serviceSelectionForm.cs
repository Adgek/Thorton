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
            logger.Log(LogLevel.Info, "=======================================================");
            logger.Log(LogLevel.Info, "                -- USER APP LOG --");
            logger.Log(LogLevel.Info, "Team : " + TeamName + " (Kyle F, Adrian K, Matthew A)"); 
            logger.Log(LogLevel.Info, "======================================================="); 
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
                    HL7 messageToSend = handler.RegisterTeamMessage(service);
                    LogSendSOARegistryCallStart(messageToSend);

                    returnMsg = handler.HandleResponse(SocketSender.StartClient(messageToSend.fullHL7Message, registryIP, registryPort));
                    LogSendSOARegistryCallEnd(returnMsg);
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

                    HL7 messageToSend = handler.QueryServiceMessage(service);
                    LogSendSOARegistryCallStart(messageToSend);
                    returnMsg = handler.HandleResponse(SocketSender.StartClient(messageToSend.fullHL7Message, registryIP, registryPort));
                    LogSendSOARegistryCallEnd(returnMsg);
                    fields = returnMsg.segments.FirstOrDefault().fields;

                    if (fields[1] == "OK")
                    {
                        this.Hide();
                        serviceCallerForm sc = new serviceCallerForm(returnMsg, service.TeamName, service.TeamID, this);
                        sc.Show();
                    }
                    else
                    {
                        MessageBox.Show("DATA NOT VALID!! Could not query a valid service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    } 
                }
                else
                {
                    MessageBox.Show("DATA NOT VALID!! Could not find a team registered under the name: " + TeamName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }                
            }            
        }

        private static void LogSendSOARegistryCallEnd(HL7 returnMsg)
        {
            logger.Log(LogLevel.Info, "\t >> Response from SOA-Registry :");
            foreach (HL7Segment seg in returnMsg.segments)
            {
                logger.Log(LogLevel.Info, "\t\t >> " + seg.segment);
            }
            
            logger.Log(LogLevel.Info, "---");
        }

        private static void LogSendSOARegistryCallStart(HL7 messageToSend)
        {
            logger.Log(LogLevel.Info, "---");
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :");
            foreach (HL7Segment seg in messageToSend.segments)
            {
                logger.Log(LogLevel.Info, "\t >> " + seg.segment);
            }
        }
    }
}

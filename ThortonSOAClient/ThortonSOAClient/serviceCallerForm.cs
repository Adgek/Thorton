using FluentValidation.Results;
using HL7Lib.HL7;
using HL7Lib.ServiceData;
using NLog;
using RegistryHandlerLib;
using SocketClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThortonSOAClient.UI_Elements;

namespace ThortonSOAClient
{
    public partial class serviceCallerForm : Form
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<UiArgument> argumentElements = new List<UiArgument>();

        private HL7 ServiceInformation = new HL7();

        private static int yLocation = 10;

        private static List<HL7Lib.ServiceData.Message> responseDefinitions = new List<HL7Lib.ServiceData.Message>();

        private static HL7Handler handler = new HL7Handler();

        private string ServiceIp = "";
        private int ServicePort = 0;

        private string OurTeamName = "";
        private string OurTeamId = "";

        private static string registryIP = ConfigurationManager.AppSettings["registryipaddress"];
        private static int registryPort = Convert.ToInt32(ConfigurationManager.AppSettings["registryport"]);

        private Form origForm;

        private void RegisterTeam()
        {


            Service  service = new Service();
            service.TeamName = OurTeamName;
            HL7 returnMsg;
            try
            {
                LogSendSOARegistryCallStart(handler.RegisterTeamMessage(service));
                returnMsg = RegistryCommunicator.RegisterTeam(service, registryPort, registryIP);
                LogSendSOARegistryCallEnd(returnMsg);
                string ErrorMessage = returnMsg.Validate();
                if (ErrorMessage != "valid")
                {
                    MessageBox.Show("DATA NOT VALID!! Error: " + ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    logger.Log(LogLevel.Fatal, ErrorMessage);
                    return;
                }
                    
            }
            catch(Exception ex)
            {
                MessageBox.Show("DATA NOT VALID!! Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, ex.Message);
                return;
            }
            string ErrorMessage2 = returnMsg.Validate();
            if (ErrorMessage2 != "valid")
            {
                MessageBox.Show("DATA NOT VALID!! Error: " + ErrorMessage2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, ErrorMessage2);
                return;
            }
            List<string> fields = returnMsg.segments.FirstOrDefault().fields;
            if (fields[1] == "OK")
            {
                OurTeamId = fields[2];
            }
            else
            {
                MessageBox.Show("DATA NOT VALID!! Could not find a team registered under the name: " + OurTeamName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, "DATA NOT VALID!! Could not find a team registered under the name: " + OurTeamName);
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

        private List<ValidationResult> results = new List<ValidationResult>();
    
        public serviceCallerForm()
        {
            InitializeComponent();
        }
        

        public serviceCallerForm(HL7 message, string teamName, string id, Form firstPage)
        {
            InitializeComponent();
            ServiceInformation = message;
            OurTeamName = teamName;
            OurTeamId = id;
            origForm = firstPage;
        }

        private void serviceCaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            origForm.Show();
        }

        private void serviceCallerForm_Load(object sender, EventArgs e)
        {
            ReadServiceDataAndInitArgInputArea();
        }

        private void ReadServiceDataAndInitArgInputArea()
        {
            teamNametb.Text = ServiceInformation.segments[1].fields[1];
            serviceNameTB.Text = ServiceInformation.segments[1].fields[2];
            serviceDescTB.Text = ServiceInformation.segments[1].fields[6];
            int x = 2;
            List<List<string>> argInfo = new List<List<string>>();

            if (ServiceInformation.segments[x].fields[0] == "ARG")
            {
                for (; ServiceInformation.segments[x].fields[0] == "ARG"; x++)
                {
                    argInfo.Add(ServiceInformation.segments[x].fields);
                }
                CreateInputArea(argInfo);
            }
            if (ServiceInformation.segments[x].fields[0] == "RSP")
            {
                HL7Lib.ServiceData.Message r;
                for (; ServiceInformation.segments[x].fields[0] == "RSP"; x++)
                {
                    r = new HL7Lib.ServiceData.Message(Convert.ToInt32(ServiceInformation.segments[x].fields[1]), ServiceInformation.segments[x].fields[2], ServiceInformation.segments[x].fields[3]);
                }
            }
            ServiceIp = ServiceInformation.segments[x].fields[1];
            ServicePort = Convert.ToInt32(ServiceInformation.segments[x].fields[2]);
        }

        private void CreateInputArea(List<List<string>> argInfo)
        {
            UiArgument a;
            for (int x = 0; x < argInfo.Count; x++)
            {
                a = new UiArgument(Convert.ToInt32(argInfo[x][1]), argInfo[x][2], argInfo[x][3], argInfo[x][4] == "mandatory" ? true : false);
                argumentElements.Add(a);
                a.Lbl.Location = new Point(0, yLocation);
                a.Tb.Location = new Point(0, yLocation + 25);
                argPanel.Controls.Add(a.Tb);
                argPanel.Controls.Add(a.Lbl);
                yLocation += 65;
            }
        }
        const char BEGIN_MESSAGE = (char)11;
        const char END_SEGMENT = (char)13;
        const char END_MESSAGE = (char)28;

        private void executeBtn_Click(object sender, EventArgs e)
        {
            Service service = new Service(OurTeamName,OurTeamId);
            if (GetArgumentValuesAndValidateInput(service))
            {
                RegisterTeam();
                service.ServiceName = (serviceNameTB.Text);
                HL7 cmd = handler.ExecuteServiceMessage(service);
                logger.Log(LogLevel.Info, "---");
                logger.Log(LogLevel.Info, "Sending service request to IP " + ServiceIp + ", PORT " +ServicePort +" :");
                LogSendServiceCall(cmd);
                string rep = "";
                try
                {
                    rep = SocketSender.StartClient(cmd.fullHL7Message, ServiceIp, ServicePort);
                }
                catch(Exception ex)
                {
                    logger.Log(LogLevel.Fatal,ex.Message);
                    return;
                }
                HL7 returnMsg = handler.HandleResponse(rep);
                LogSendServiceCallEnd(returnMsg);
                DisplayOutput(returnMsg);
                ClearErrors();
            }
            else
            {
                ShowErrors();
            }
        }

        private static void LogSendServiceCall(HL7 msg)
        {
            foreach (HL7Segment seg in msg.segments)
            {
                logger.Log(LogLevel.Info, "\t >> " + seg.segment);
            }

            logger.Log(LogLevel.Info, "---");
        }

        private static void LogSendServiceCallEnd(HL7 returnMsg)
        {
            logger.Log(LogLevel.Info, "\t >> Response from Published Service :");
            foreach (HL7Segment seg in returnMsg.segments)
            {
                logger.Log(LogLevel.Info, "\t\t >> " + seg.segment);
            }

            logger.Log(LogLevel.Info, "---");
        }

        private void ClearErrors()
        {
            foreach(UiArgument arg in argumentElements)
            {
                arg.Err.Clear();
            }
        }

        private void ShowErrors()
        {
            responseTB.AppendText( "|-----------------------------------------------------|" + Environment.NewLine);
            responseTB.AppendText( "|                          Errors                              |" + Environment.NewLine);
            responseTB.AppendText( "|-----------------------------------------------------|" + Environment.NewLine);
            for (int x = 0; x < results.Count;x++ )
            {

                if (results[x].IsValid)
                    argumentElements[x].Err.Clear();
                foreach (ValidationFailure fail in results[x].Errors)
                {   
                    
                    argumentElements[x].Err.SetError(argumentElements[x].Tb, fail.ErrorMessage);
                    responseTB.AppendText( "The data for the message " + argumentElements[x].Name + " '" + argumentElements[x].Tb.Text + "' was not valid. Error: " + fail.ErrorMessage + Environment.NewLine);
                }
            }
        }

        private void DisplayOutput(HL7 returnMsg)
        {
            if (returnMsg.segments.Count < 1)
            {
                responseTB.AppendText("Error: No valid HL7 messages were returned to display." + Environment.NewLine + Environment.NewLine);
                return;
            }
            string ErrorMessage = returnMsg.Validate();
            if(ErrorMessage != "valid")
            {
                responseTB.AppendText("Error: " + ErrorMessage + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, ErrorMessage);
                return;
            }
            responseTB.AppendText("|-----------------------------------------------------|" + Environment.NewLine);
            responseTB.AppendText("|                    Response                            |" + Environment.NewLine);
            responseTB.AppendText("|-----------------------------------------------------|" + Environment.NewLine);
            if (returnMsg.segments[0].fields[0] != "PUB" || returnMsg.segments[0].fields[1] != "OK")
            {
                if (returnMsg.segments[0].fields[3] == "")
                {

                    responseTB.AppendText("Error: Unexpected message was returned from the service and no error was provided." + Environment.NewLine + Environment.NewLine);
                    logger.Log(LogLevel.Fatal, "Error: Unexpected message was returned from the service and no error was provided.");
                }
                else
                {

                    responseTB.AppendText("Error thrown the service. Message provided was: " + returnMsg.segments[0].fields[3] + Environment.NewLine + Environment.NewLine);
                    logger.Log(LogLevel.Fatal, "Error thrown the service. Message provided was: " + returnMsg.segments[0].fields[3]);
                }
                return;
            }

            int numResponses;
            try
            {
                numResponses = Convert.ToInt32(returnMsg.segments[0].fields[4]);
            }
            catch
            {
                responseTB.AppendText("Error: Number of responses was not provided. " + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, "Error: Number of responses was not provided. ");
                
                return;
            }
            int x = 1;
            if (returnMsg.segments.Count -1 < x)
            {
                responseTB.AppendText("Error: No response was found to display. " + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, "Error: No response was found to display. ");
                return;
            }
            if (returnMsg.segments[x].fields[0] == "RSP")
            {
                for (; x < numResponses + 1 && !(x >returnMsg.segments.Count -1); x++)
                {
                    if (returnMsg.segments[x].fields[0] == "RSP")
                    { 
                        responseTB.AppendText( "RSP " + x + Environment.NewLine);
                        responseTB.AppendText( " Return Field: " + returnMsg.segments[x].fields[2] + Environment.NewLine);
                        responseTB.AppendText( " Return Value: " + returnMsg.segments[x].fields[4] + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        logger.Log(LogLevel.Warn, "Error: Found invalid segment before response parsing was complete. Segment:  " + returnMsg.segments[x].segment + Environment.NewLine + Environment.NewLine);
                        break;
                    }
                }
            }
            else
            {
                responseTB.AppendText("Error: No response was found to display. " + Environment.NewLine + Environment.NewLine);
            }
            if(returnMsg.segments.Count-1 != numResponses)
            {
                logger.Log(LogLevel.Warn, "Error: Service provided less responses than specified. Expected: " + numResponses + " Recieved: " + (returnMsg.segments.Count - 1));
            }            
        }

        private Boolean GetArgumentValuesAndValidateInput(Service serv)
        {
            HL7Lib.ServiceData.Message arg;
            if (!ValidateArgs(argumentElements))
                return false;
            foreach(UiArgument Argument in argumentElements)
            {
                string value = Argument.Tb.Text;
                
                arg = new HL7Lib.ServiceData.Message(Argument.Position, Argument.Name, Argument.DataType, Argument.Mandatory, value);
                serv.Arguments.Add(arg);
            }
            return true;
        }

        private Boolean ValidateArgs(List<UiArgument> argumentElements)
        {
            results.Clear();
            foreach (UiArgument Argument in argumentElements)
            {
                Argument.Value = Argument.Tb.Text;                    
                results.Add(Argument.Validator.Validate(Argument));
            }

            foreach(ValidationResult vr in results)
            {
                if (!vr.IsValid)
                    return false;
            }
            return true;
        }
    }
}

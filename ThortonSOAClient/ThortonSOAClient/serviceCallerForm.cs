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
    /// <summary>
    /// Service calling UI
    /// </summary>
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

        private List<ValidationResult> results = new List<ValidationResult>();

        /// <summary>
        /// Method to register our team. Takes no arguments because all the information needed
        /// is pulled from app config and is internally available to this calss
        /// </summary>
        private void RegisterTeam()
        {
            Service  service = new Service();
            service.TeamName = OurTeamName;
            HL7 returnMsg;
            try // try to call the SOA registry to register the team
            {
                LogSendSOARegistryCallStart(handler.RegisterTeamMessage(service));
                returnMsg = RegistryCommunicator.RegisterTeam(service, registryPort, registryIP);
                LogSendSOARegistryCallEnd(returnMsg);                    
            }
            catch(Exception ex) // catch the error from the socket class and inform the user if something goes wrongs
            {
                MessageBox.Show("DATA NOT VALID!! Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, ex.Message);
                return;
            }
            string ErrorMessage2 = returnMsg.Validate();
            if (ErrorMessage2 != "valid") // display the error to the user if the response from the SOA registry was invalid
            {
                MessageBox.Show("DATA NOT VALID!! Error: " + ErrorMessage2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, ErrorMessage2);
                return;
            }
            List<string> fields = returnMsg.segments.FirstOrDefault().fields;
            if (fields[1] == "OK") // if the message was ok, extract the team id
            {
                OurTeamId = fields[2];
            }
            else// else display an error
            {
                MessageBox.Show("DATA NOT VALID!! Could not find a team registered under the name: " + OurTeamName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                logger.Log(LogLevel.Fatal, "DATA NOT VALID!! Could not find a team registered under the name: " + OurTeamName);
            }  
        }

        /// <summary>
        /// log in sean's specified format
        /// </summary>
        /// <param name="returnMsg">the message returned to log</param>
        private static void LogSendSOARegistryCallEnd(HL7 returnMsg)
        {
            logger.Log(LogLevel.Info, "\t >> Response from SOA-Registry :");
            foreach (HL7Segment seg in returnMsg.segments)
            {
                logger.Log(LogLevel.Info, "\t\t >> " + seg.segment);
            }

            logger.Log(LogLevel.Info, "---");
        }

        /// <summary>
        /// log in sean's specified format
        /// </summary>
        /// <param name="returnMsg">the message returned to log</param>
        private static void LogSendSOARegistryCallStart(HL7 messageToSend)
        {
            logger.Log(LogLevel.Info, "---");
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :");
            foreach (HL7Segment seg in messageToSend.segments)
            {
                logger.Log(LogLevel.Info, "\t >> " + seg.segment);
            }
        }
    
        public serviceCallerForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// secondary form constructor
        /// </summary>
        /// <param name="message">service information</param>
        /// <param name="teamName">our team name</param>
        /// <param name="id">our team id</param>
        /// <param name="firstPage"> reference to the first page</param>
        public serviceCallerForm(HL7 message, string teamName, string id, Form firstPage)
        {
            InitializeComponent();
            ServiceInformation = message;
            OurTeamName = teamName;
            OurTeamId = id;
            origForm = firstPage;
        }
        
        /// <summary>
        /// on form close, show first form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceCaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            origForm.Show();
        }

        /// <summary>
        /// on form load, initialize the UI
        /// </summary>
        /// <param name="sender">event arg</param>
        /// <param name="e">event arg</param>
        private void serviceCallerForm_Load(object sender, EventArgs e)
        {
            ReadServiceDataAndInitArgInputArea();
        }

        /// <summary>
        /// initialize the UI
        /// </summary>
        private void ReadServiceDataAndInitArgInputArea()
        {
            //fill in service data
            teamNametb.Text = ServiceInformation.segments[1].fields[1];
            serviceNameTB.Text = ServiceInformation.segments[1].fields[2];
            serviceDescTB.Text = ServiceInformation.segments[1].fields[6];
            int x = 2;
            //dynamically build up the input form
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

        /// <summary>
        /// create the input form
        /// </summary>
        /// <param name="argInfo"> the info about the arguments on the form</param>
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

        /// <summary>
        /// execute the call to the service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executeBtn_Click(object sender, EventArgs e)
        {
            Service service = new Service(OurTeamName,OurTeamId);
            if (GetArgumentValuesAndValidateInput(service))
            {
                // register team to ensure our team still exists.  Thanks Sean -_-
                RegisterTeam();

                // prepare message to service
                service.ServiceName = (serviceNameTB.Text);
                HL7 cmd = handler.ExecuteServiceMessage(service);
                logger.Log(LogLevel.Info, "---");
                logger.Log(LogLevel.Info, "Sending service request to IP " + ServiceIp + ", PORT " +ServicePort +" :");
                LogSendServiceCall(cmd);
                string rep = "";
                try
                {
                    rep = SocketSender.StartClient(cmd.fullHL7Message, ServiceIp, ServicePort); // call the service
                }
                catch(Exception ex)
                {
                    logger.Log(LogLevel.Fatal,ex.Message);
                    return;
                }
                HL7 returnMsg = handler.HandleResponse(rep); // handle the response
                LogSendServiceCallEnd(returnMsg);
                DisplayOutput(returnMsg); // display response
                ClearErrors();
            }
            else
            {
                ShowErrors();
            }
        }

        /// <summary>
        /// log information about what is sent to the service
        /// </summary>
        /// <param name="msg">the message being sent</param>
        private static void LogSendServiceCall(HL7 msg)
        {
            foreach (HL7Segment seg in msg.segments)
            {
                logger.Log(LogLevel.Info, "\t >> " + seg.segment);
            }

            logger.Log(LogLevel.Info, "---");
        }

        /// <summary>
        /// log response from the service
        /// </summary>
        /// <param name="returnMsg">the message returned from the service</param>
        private static void LogSendServiceCallEnd(HL7 returnMsg)
        {
            logger.Log(LogLevel.Info, "\t >> Response from Published Service :");
            foreach (HL7Segment seg in returnMsg.segments)
            {
                logger.Log(LogLevel.Info, "\t\t >> " + seg.segment);
            }

            logger.Log(LogLevel.Info, "---");
        }

        /// <summary>
        /// clear errors on the ui
        /// </summary>
        private void ClearErrors()
        {
            foreach(UiArgument arg in argumentElements)
            {
                arg.Err.Clear();
            }
        }
        
        /// <summary>
        /// display input validation errors to the user
        /// </summary>
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

        /// <summary>
        /// display response from a service
        /// </summary>
        /// <param name="returnMsg">return value from the service</param>
        private void DisplayOutput(HL7 returnMsg)
        {
            // if no reponse were given, inform the user/log it
            if (returnMsg.segments.Count < 1)
            {
                responseTB.AppendText("Error: No valid HL7 messages were returned to display." + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, "Error: No valid HL7 messages were returned to display.");
                return;
            }
            string ErrorMessage = returnMsg.Validate();
            if(ErrorMessage != "valid") // if some segments were invalid, inform the user/log it
            {
                responseTB.AppendText("Error: " + ErrorMessage + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, ErrorMessage);
                return;
            }
            responseTB.AppendText("|-----------------------------------------------------|" + Environment.NewLine);
            responseTB.AppendText("|                    Response                            |" + Environment.NewLine);
            responseTB.AppendText("|-----------------------------------------------------|" + Environment.NewLine);
            if (returnMsg.segments[0].fields[0] != "PUB" || returnMsg.segments[0].fields[1] != "OK") // if we are missing the PUB message, inform the user
            {
                if (returnMsg.segments[0].fields[3] == "") // if the message doesn't include the required data, inform the user/log it
                {

                    responseTB.AppendText("Error: Unexpected message was returned from the service and no error was provided." + Environment.NewLine + Environment.NewLine);
                    logger.Log(LogLevel.Fatal, "Error: Unexpected message was returned from the service and no error was provided.");
                }
                else // if the message contains an error we can display, display/log it
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
            catch // if num responses isn't a valid int, inform the user and log it
            {
                responseTB.AppendText("Error: Number of responses was not provided. " + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, "Error: Number of responses was not provided. ");
                
                return;
            }
            int x = 1;
            if (returnMsg.segments.Count -1 < x)
            {
                // if no response was returned, inform the user and log it
                responseTB.AppendText("Error: No response was found to display. " + Environment.NewLine + Environment.NewLine);
                logger.Log(LogLevel.Fatal, "Error: No response was found to display. ");
                return;
            }
            if (returnMsg.segments[x].fields[0] == "RSP")
            {
                for (; x < numResponses + 1 && !(x >returnMsg.segments.Count -1); x++)
                {
                    // display responses
                    if (returnMsg.segments[x].fields[0] == "RSP")
                    { 
                        responseTB.AppendText( "RSP " + x + Environment.NewLine);
                        responseTB.AppendText( " Return Field: " + returnMsg.segments[x].fields[2] + Environment.NewLine);
                        responseTB.AppendText( " Return Value: " + returnMsg.segments[x].fields[4] + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {
                        // if an invalid response was found, log it but continue to parse responses
                        logger.Log(LogLevel.Warn, "Error: Found invalid segment before response parsing was complete. Segment:  " + returnMsg.segments[x].segment + Environment.NewLine + Environment.NewLine);
                        break;
                    }
                }
            }
            else
            {
                responseTB.AppendText("Error: No response was found to display. " + Environment.NewLine + Environment.NewLine);
            }
            if(returnMsg.segments.Count-1 != numResponses) // if the number of responses the service said it would return doesnt match what we got, log it
            {
                logger.Log(LogLevel.Warn, "Error: Service provided less responses than specified. Expected: " + numResponses + " Recieved: " + (returnMsg.segments.Count - 1));
            }            
        }

        /// <summary>
        /// validate arguments input by the user
        /// </summary>
        /// <param name="serv">the service object to add the arguments to</param>
        /// <returns>true = valid, false = invalid</returns>
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

        /// <summary>
        /// validate the arguments
        /// </summary>
        /// <param name="argumentElements">arguments to validate</param>
        /// <returns>true = validate, false = invalid</returns>
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

using FluentValidation.Results;
using HL7Lib.HL7;
using HL7Lib.ServiceData;
using SocketClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static List<UiArgument> argumentElements = new List<UiArgument>();

        private HL7 ServiceInformation = new HL7();

        private static int yLocation = 10;

        private static List<HL7Lib.ServiceData.Message> responseDefinitions = new List<HL7Lib.ServiceData.Message>();

        private static HL7Handler handler = new HL7Handler();

        private string ServiceIp = "";
        private int ServicePort = 0;

        private string OurTeamName = "";
        private string OurTeamId = "";

        private Form origForm;

        private List<ValidationResult> results = new List<ValidationResult>();
    
        public serviceCallerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// validate string for bad chars
        /// </summary>
        /// <param name="test">string to check</param>
        /// <returns></returns>
        

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

        private void executeBtn_Click(object sender, EventArgs e)
        {
            Service service = new Service(OurTeamName,OurTeamId);
            if (GetArgumentValuesAndValidateInput(service))
            {
                service.ServiceName = (serviceNameTB.Text);
                string cmd = handler.ExecuteServiceMessage(service);
                string rep = SocketSender.StartClient(cmd, ServiceIp, ServicePort);
                HL7 returnMsg = handler.HandleResponse(rep);
                DisplayOutput(returnMsg);
                ClearErrors();
            }
            else
            {
                ShowErrors();
            }
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
            if (returnMsg.segments[0].fields[0] != "PUB" || returnMsg.segments[0].fields[1] != "OK")
            {
                // error out
            }
            int numResponses = Convert.ToInt32(returnMsg.segments[0].fields[4]);
            int x = 1;
            if (returnMsg.segments[x].fields[0] == "RSP")
            {
                responseTB.AppendText( "|-----------------------------------------------------|" + Environment.NewLine);
                responseTB.AppendText( "|                    Response                            |" + Environment.NewLine);
                responseTB.AppendText( "|-----------------------------------------------------|" + Environment.NewLine);
                for (; x < numResponses + 1; x++)
                {
                    responseTB.AppendText( "RSP " + x + Environment.NewLine);
                    responseTB.AppendText( " Return Field: " + returnMsg.segments[x].fields[2] + Environment.NewLine);
                    responseTB.AppendText( " Return Value: " + returnMsg.segments[x].fields[4] + Environment.NewLine + Environment.NewLine);
                }
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

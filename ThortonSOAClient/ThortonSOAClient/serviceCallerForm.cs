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

        private static List<Response> responseDefinitions = new List<Response>();

        private static HL7Handler handler = new HL7Handler();

        private string ServiceIp = "";
        private int ServicePort = 0;

        private string OurTeamName = "";
        private string OurTeamId = "";

    
        public serviceCallerForm()
        {
            InitializeComponent();
        }

        public serviceCallerForm(HL7 message, string teamName, string id)
        {
            InitializeComponent();
            ServiceInformation = message;
            OurTeamName = teamName;
            OurTeamId = id;
        }

        private void serviceCaller_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
                Response r;
                for (; ServiceInformation.segments[x].fields[0] == "RSP"; x++)
                {
                    r = new Response(Convert.ToInt32(ServiceInformation.segments[x].fields[1]), ServiceInformation.segments[x].fields[2], ServiceInformation.segments[x].fields[3]);
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
            Boolean isValid = GetArgumentValuesAndValidateInput(service);
            if (isValid)
            {
                service.ServiceName = (serviceNameTB.Text);
                string cmd = handler.ExecuteServiceMessage(service);
                string rep = SocketSender.StartClient(cmd, ServiceIp, ServicePort);
                HL7 returnMsg = handler.HandleResponse(rep);
                DisplayOutput(returnMsg);
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
                responseTB.Text += "|-----------------------------------------------------|" + Environment.NewLine;
                responseTB.Text += "|                    Response                            |" + Environment.NewLine;
                responseTB.Text += "|-----------------------------------------------------|" + Environment.NewLine;
                for (; x < numResponses + 1; x++)
                {
                    responseTB.Text += "RSP " + x + Environment.NewLine;
                    responseTB.Text += " Return Field: " + returnMsg.segments[x].fields[2] + Environment.NewLine;
                    responseTB.Text += " Return Value: " + returnMsg.segments[x].fields[4] + Environment.NewLine + Environment.NewLine;
                }
            }
        }

        private Boolean GetArgumentValuesAndValidateInput(Service serv)
        {
            Boolean valid = true;
            Argument arg;
            foreach(UiArgument Argument in argumentElements)
            {
                string value = Argument.Tb.Text;
                    //if valid, call method
                arg = new Argument(Argument.Position, Argument.ArgumentName, Argument.ArgumentDataType, Argument.Mandatory, value);
                serv.Arguments.Add(arg);
            }
            return valid;
        }
    }
}

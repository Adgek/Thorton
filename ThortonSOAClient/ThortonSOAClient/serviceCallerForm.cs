using HL7Lib.HL7;
using HL7Lib.ServiceData;
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

        private string ServiceIp = "";
        private string ServicePort = "";

        public serviceCallerForm()
        {
            InitializeComponent();
        }

        public serviceCallerForm(HL7 message)
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
            ServicePort = ServiceInformation.segments[x].fields[2];

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
    }
}

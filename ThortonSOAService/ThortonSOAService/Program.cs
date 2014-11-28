using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Records;
using System.Threading;
using SocketClass;
using System.Net;

namespace ThortonSOAService
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            HL7Handler handler = new HL7Handler();

            const string SERVICE_NAME = "ThortonSOAService";
            const string TEAM_NAME = "FunnyGlasses";
            const int PORT = 11000;

            string TEAM_ID = "";

            string command = "";
            string ret = "";

            //Service Start
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "Team\t: FunnyGlasses (Matt, Adrian, Kyle)\n");
            logger.Log(LogLevel.Info, "Tag-Name\t: GIORP-TOTAL\n");
            logger.Log(LogLevel.Info, "Service\t: ThortonSOAService\n");
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "---\n");                   
         
            //Register team
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            command = handler.RegisterTeamMessage();
            ret = SocketSender.StartClient(command);

            HL7 hl7 = handler.HandleResponse(ret);
            if(hl7.segments[0].fields[1] != "OK")
            {
                //throw error
            }
            TEAM_ID = hl7.segments[0].fields[2];

            logger.Log(LogLevel.Info, "---\n");

            //Publish service
            string id = "1186";
            PurchaseTotaller pt = new PurchaseTotaller();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Service service = new Service(SERVICE_NAME, TEAM_NAME, id, PurchaseTotaller.TAG_NAME, PurchaseTotaller.SECURITY_LEVEL, PurchaseTotaller.DESCRIPTION, pt.arguments, pt.responses, ipAddress, PORT);

            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            command = handler.PublishServiceMessage(service);
            ret = SocketSender.StartClient(command);

            hl7 = handler.HandleResponse(ret);
            if (hl7.segments[0].fields[1] != "OK")
            {
                //throw error
            }

            logger.Log(LogLevel.Info, "---\n");

            //Start listening for connections
            SocketListener sl = new SocketListener();
            Thread listener = new Thread(new System.Threading.ThreadStart(sl.StartListening));
            listener.Start();           
        }       
    }
}

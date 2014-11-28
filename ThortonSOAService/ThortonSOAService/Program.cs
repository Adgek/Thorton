using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Records;
using System.Threading;

namespace ThortonSOAService
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            HL7Handler handler = new HL7Handler();
            SocketListener sl = new SocketListener();
            Thread listener = new Thread(new System.Threading.ThreadStart(sl.StartListening));
            listener.Start();

            //Service Start
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "Team\t: FunnyGlasses (Matt, Adrian, Kyle)\n");
            logger.Log(LogLevel.Info, "Tag-Name\t: GIORP-TOTAL\n");
            logger.Log(LogLevel.Info, "Service\t: ThortonSOAService\n");
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "---\n");
            
            string command = "";
            
            //Register team
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            command = handler.RegisterTeamMessage();
            SocketSender.StartClient(command);

            logger.Log(LogLevel.Info, "---\n");

            //Publish service
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");

            char msgBeg = (char)11;
            char segEnd = (char)13;
            char msgEnd = (char)28;

            command = msgBeg.ToString() + 
                        "DRC|PUB-SERVICE|FunnyGlasses|1186|" + segEnd.ToString() +
                        "SRV|GIORP-TOTAL|ThortonSOAService|3|2|1|comin in hot|" + segEnd.ToString() +
                        "ARG|1|province|string|mandatory||" + segEnd.ToString() +
                        "ARG|2|principal|double|mandatory||" + segEnd.ToString() +
                        "RSP|1|total|double||" + segEnd.ToString() +
                        "MCH|192.168.2.24|11000|" + segEnd.ToString() +
                        msgEnd.ToString() + segEnd.ToString();


            SocketSender.StartClient(command);

            logger.Log(LogLevel.Info, "---\n");

            //SocketListener.StartListening();
        }
    }
}

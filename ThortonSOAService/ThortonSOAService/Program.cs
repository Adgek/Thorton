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
            command = handler.PublishServiceMessage();
            SocketSender.StartClient(command);

            logger.Log(LogLevel.Info, "---\n");

            SocketListener sl = new SocketListener();
            Thread listener = new Thread(new System.Threading.ThreadStart(sl.StartListening));
            listener.Start();
            //SocketListener.StartListening();
        }
    }
}

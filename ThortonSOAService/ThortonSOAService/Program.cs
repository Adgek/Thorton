using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThortonSOAService
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
<<<<<<< HEAD
            logger.Log(LogLevel.Info, "The program has started!");
=======
            string command = "";
            //Service Start
            
            //Register team
            command = "";
            SocketSender.StartClient(command);

            //Publish service
            command = "";
            SocketSender.StartClient(command);

            //listen for requests
            
>>>>>>> origin/master
            SocketListener.StartListening();
        }
    }
}

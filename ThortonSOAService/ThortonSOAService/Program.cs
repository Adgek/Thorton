using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThortonSOAService
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            //Service Start
            
            //Register team
            command = "";
            SocketSender.StartClient(command);

            //Publish service
            command = "";
            SocketSender.StartClient(command);

            //listen for requests
            
            SocketListener.StartListening();
            
            
        }
    }
}

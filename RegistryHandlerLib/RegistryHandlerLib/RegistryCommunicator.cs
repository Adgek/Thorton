using HL7Lib.HL7;
using HL7Lib.ServiceData;
using SocketClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryHandlerLib
{
    public static class RegistryCommunicator
    {
        private static HL7Handler handler = new HL7Handler();

        public static HL7 RegisterTeam(Service service, int registryPort, string registryIP)
        {
            HL7 messageToSend = handler.RegisterTeamMessage(service);
            return handler.HandleResponse(SocketSender.StartClient(messageToSend.fullHL7Message, registryIP, registryPort));            
        }

        public static HL7 QueryServiceInfo(Service service, int registryPort, string registryIP)
        {
            HL7 messageToSend = handler.QueryServiceMessage(service);
            return  handler.HandleResponse(SocketSender.StartClient(messageToSend.fullHL7Message, registryIP, registryPort));
        }
    }
}

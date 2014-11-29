using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Lib;
using System.Threading;
using SocketClass;
using System.Net;
using HL7Lib.HL7;
using HL7Lib.ServiceData;
using System.Configuration;

namespace ThortonSOAService
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            HL7Handler handler = new HL7Handler();

            string SERVICE_NAME = ConfigurationManager.AppSettings["ServiceName"];
            string SERVICE_IP = ConfigurationManager.AppSettings["ServiceIP"];
            string SERVICE_PORT = ConfigurationManager.AppSettings["ServicePort"];
            string REGISTRY_IP = ConfigurationManager.AppSettings["RegistryIP"];
            string REGISTRY_PORT = ConfigurationManager.AppSettings["RegistryPort"];
            string TEAM_NAME = ConfigurationManager.AppSettings["TeamName"];
            string TAG_NAME = ConfigurationManager.AppSettings["TagName"];

            int Port = 0;
            try 
            {
                int.TryParse(SERVICE_PORT, out Port);
            }
            catch(Exception e){
                logger.Log(LogLevel.Error, "Invalid service port read from config file: " + e.Message);
                Console.WriteLine("Invalid service port read from config file. Exiting.");
                return;
            }            
            
            int RegistryPort = 0;            
            try
            {
                int.TryParse(REGISTRY_PORT, out RegistryPort);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Invalid registry port read from config file: " + e.Message);
                Console.WriteLine("Invalid registry port read from config file. Exiting.");
                return;
            }

            string TEAM_ID = "";

            string command = "";
            string ret = "";

            //Service Start
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "Team\t: " + TEAM_NAME + "(Matt, Adrian, Kyle)\n");
            logger.Log(LogLevel.Info, "Tag-Name : " + TAG_NAME + "\n");
            logger.Log(LogLevel.Info, "Service\t: " + SERVICE_NAME + "\n");
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "---\n");                   
         
            //Register team
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            Service register = new Service();
            register.TeamName = TEAM_NAME;
            command = handler.RegisterTeamMessage(register);
            logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(command) + "\n");
            ret = SocketSender.StartClient(command, REGISTRY_IP, RegistryPort);
            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            logger.Log(LogLevel.Info, "\t\t>> " + HL7Parser.LogSegment(ret) + "\n");

            if (ret.Contains("SOA"))
            {
                HL7 hl7 = handler.HandleResponse(ret);

                if (hl7.segments[0].fields[1] != "OK")
                {
                    logger.Log(LogLevel.Error, "Could not register the team");
                    Console.WriteLine("Could not register the team");
                    return;
                }
                TEAM_ID = hl7.segments[0].fields[2];
            }
            else
            {
                logger.Log(LogLevel.Error, "Invalid response from registry");
                Console.WriteLine("Invalid response from registry");
                return;
            }

            logger.Log(LogLevel.Info, "---\n");

            //Publish service
            PurchaseTotaller pt = new PurchaseTotaller();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse(SERVICE_IP);
            Service service = new Service(SERVICE_NAME, TEAM_NAME, TEAM_ID, PurchaseTotaller.TAG_NAME, PurchaseTotaller.SECURITY_LEVEL, PurchaseTotaller.DESCRIPTION, pt.arguments, pt.responses, ipAddress, Port);

            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            command = handler.PublishServiceMessage(service);
            logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(command) + "\n");
            ret = SocketSender.StartClient(command, REGISTRY_IP, RegistryPort);
            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            logger.Log(LogLevel.Info, "\t\t>> " + HL7Parser.LogSegment(ret) + "\n");

            if (ret.Contains("SOA"))
            {
                HL7 hl7 = handler.HandleResponse(ret);
                if (hl7.segments[0].fields[1] != "OK")
                {
                    if (ret.Contains("has already published service"))
                    {
                        logger.Log(LogLevel.Error, "The service has already been published");
                        Console.WriteLine("The service has already been publish. Proceeding.");
                    }
                    else
                    {
                        logger.Log(LogLevel.Error, "Could not publish the service");
                        Console.WriteLine("Could not publish the service");
                        return;
                    }                  
                }                
            }
            else
            {
                logger.Log(LogLevel.Error, "Invalid response from registry");
                Console.WriteLine("Invalid response from registry");
                return;
            }
            logger.Log(LogLevel.Info, "---\n");

            //Start listening for connections
            SocketListener sl = new SocketListener();
            Thread listener = new Thread(new System.Threading.ThreadStart(sl.StartListening));
            try
            {
                listener.Start(); 
            }
            catch(Exception e){
                logger.Log(LogLevel.Error, "Service failed starting: " + e.Message);
                Console.WriteLine("Service failed starting");
                return;
            }
        }       
    }
}

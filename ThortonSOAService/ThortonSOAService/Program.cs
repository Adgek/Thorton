﻿using NLog;
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
            string TEAM_NAME = ConfigurationManager.AppSettings["TeamName"];
            string TAG_NAME = ConfigurationManager.AppSettings["TagName"];
            const int PORT = 11000;

            string TEAM_ID = "";

            string command = "";
            string ret = "";

            //Service Start
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "Team\t: " + TEAM_NAME + "(Matt, Adrian, Kyle)\n");
            logger.Log(LogLevel.Info, "Tag-Name\t: " + TAG_NAME + "\n");
            logger.Log(LogLevel.Info, "Service\t: " + SERVICE_NAME + "\n");
            logger.Log(LogLevel.Info, "==================================================================\n");
            logger.Log(LogLevel.Info, "---\n");                   
         
            //Register team
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            Service register = new Service();
            register.TeamName = TEAM_NAME;
            command = handler.RegisterTeamMessage(register);
            logger.Log(LogLevel.Info, "\t>> " + command + "\n");
            ret = SocketSender.StartClient(command);
            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            logger.Log(LogLevel.Info, "\t\t>> " + ret + "\n");

            HL7 hl7 = handler.HandleResponse(ret);
            if(hl7.segments[0].fields[1] != "OK")
            {
                //throw error
            }
            TEAM_ID = hl7.segments[0].fields[2];

            logger.Log(LogLevel.Info, "---\n");

            //Publish service
            PurchaseTotaller pt = new PurchaseTotaller();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Service service = new Service(SERVICE_NAME, TEAM_NAME, TEAM_ID, PurchaseTotaller.TAG_NAME, PurchaseTotaller.SECURITY_LEVEL, PurchaseTotaller.DESCRIPTION, pt.arguments, pt.responses, ipAddress, PORT);

            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            command = handler.PublishServiceMessage(service);
            logger.Log(LogLevel.Info, "\t>> " + command + "\n");
            ret = SocketSender.StartClient(command);
            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            logger.Log(LogLevel.Info, "\t\t>> " + ret + "\n");

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

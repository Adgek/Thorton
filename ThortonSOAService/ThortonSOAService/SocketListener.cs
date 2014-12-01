//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: SocketListener.cs
//Date: 23/11/14
//Purpose: This file contains the asyncronous service logic which is able to handle multiple clients at the same time.
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HL7Lib;
using SocketClass;
using NLog;
using System.Configuration;
using HL7Lib.HL7;
using HL7Lib.ServiceData;

namespace ThortonSOAService
{
    class SocketListener
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public int PORT;
        public string TEAM_ID;
        public string TEAM_NAME;
        public string SERVICE_TAG;
        public string SERVICE_IP;

        string RegistryIp;
        int RegistryPort;

        HL7 message;
        HL7 response;

        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public SocketListener()
        {
            logger.Log(LogLevel.Info, "Starting to listen for connections\n");
            logger.Log(LogLevel.Info, "---");

            TEAM_NAME = ConfigurationManager.AppSettings["TeamName"];
            RegistryIp = ConfigurationManager.AppSettings["RegistryIP"];

            try
            {
                int.TryParse(ConfigurationManager.AppSettings["RegistryPort"], out RegistryPort);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Invalid registry port read from config file: " + e.Message);
                Console.WriteLine("Invalid registry port read from config file. Exiting.");
                return;
            }
            
            //QUERY FOR TEAM ID
            HL7Handler hl7h = new HL7Handler(); 
            logger.Log(LogLevel.Info, "Retrieving team id...\n");
            logger.Log(LogLevel.Info, "---");
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            Service team = new Service();
            team.TeamName = TEAM_NAME;
                        
            message = hl7h.RegisterTeamMessage(team);
            LogUtility.logMessage(message);
            string ret = "";

            try
            {
                ret = SocketSender.StartClient(message.fullHL7Message, RegistryIp, RegistryPort);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Could not open socket to the regisry : " + e.Message);
                Console.WriteLine("Could not open socket to the regisry : " + e.Message);
                return;
            }
           
            response = hl7h.HandleResponse(ret);

            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            LogUtility.logMessage(response);   

            if (ret.Contains("SOA"))
            {
                if (response.segments[0].fields[1] != "OK")
                {
                    logger.Log(LogLevel.Error, "Could not retrieve team id");
                    Console.WriteLine("Could not retrieve team id");
                    return;
                }
                TEAM_ID = response.segments[0].fields[2];
            }
            else
            {
                logger.Log(LogLevel.Error, "Invalid response from registry");
                Console.WriteLine("Invalid response from registry");
                return;
            }            

            logger.Log(LogLevel.Info, "---\n");

            SERVICE_TAG = ConfigurationManager.AppSettings["TagName"];
            SERVICE_IP = ConfigurationManager.AppSettings["ServiceIP"];
            string SERVICE_PORT = ConfigurationManager.AppSettings["ServicePort"];

            try
            {
                Int32.TryParse(SERVICE_PORT, out PORT);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Invalid service port read from config file: " + e.Message);
                Console.WriteLine("Invalid service port read from config file. Exiting.");
                return;
            }            
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.   
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse(SERVICE_IP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.                
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Could not bind to socket at ip: " + localEndPoint.Address +" port: "+ localEndPoint.Port +" : " + e.Message);
                Console.WriteLine("Could not bind to socket at ip: " + localEndPoint.Address + " port: " + localEndPoint.Port + ". Exiting.");
                return;
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            logger.Log(LogLevel.Info, "Receiving service request :\n");
            HL7 clientData;

            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                content = state.sb.ToString();
                logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(content));
                logger.Log(LogLevel.Info, "---");
                //read content               
                HL7Handler hl7h = new HL7Handler();
                clientData = hl7h.HandleResponse(content);                       

                string isValid = clientData.Validate();
                if (isValid != "valid")
                {
                    Console.WriteLine("Client data is not valid");

                    logger.Log(LogLevel.Error, "Incoming data is not in a readable format" );
                    LogUtility.logMessage(clientData);

                    Service errorResponse = new Service();
                    errorResponse.errorCode = "-1";
                    errorResponse.errorMessage = "Incoming data is not in a readable format";

                    message = hl7h.BuildResponseMessage(errorResponse, true);
                    Send(handler, message.fullHL7Message);
                    return;
                }
                else if (clientData.segments.Count < 4)
                {
                    Console.WriteLine("Not enough segments to process");

                    logger.Log(LogLevel.Error, "Not enough segments to process");
                    LogUtility.logMessage(clientData);

                    Service errorResponse = new Service();
                    errorResponse.errorCode = "-1";
                    errorResponse.errorMessage = "Not enough segments to process";

                    message = hl7h.BuildResponseMessage(errorResponse, true);
                    Send(handler, message.fullHL7Message);
                    return;
                }
                if (clientData.segments[1].fields[0] != "SRV" && clientData.segments[2].fields[0] != "ARG" && clientData.segments[3].fields[0] != "ARG")
                {
                    Console.WriteLine("Incorrect segment types recieved");

                    logger.Log(LogLevel.Error, "Incorrect segment types recieved");
                    LogUtility.logMessage(clientData);

                    Service errorResponse = new Service();
                    errorResponse.errorCode = "-1";
                    errorResponse.errorMessage = "Incorrect segment types recieved";

                    message = hl7h.BuildResponseMessage(errorResponse, true);
                    Send(handler, message.fullHL7Message);
                    return;
                }   
                
                //take team info    
                //query registry           
                Service service = new Service(TEAM_NAME, TEAM_ID);
                Service teamService = new Service(clientData.segments[0].fields[2], clientData.segments[0].fields[3]);
                teamService.Tag = ConfigurationManager.AppSettings["TagName"];
                              
                message = hl7h.QueryTeamMessage(service, teamService);
                string ret = "";
                try
                {
                    ret = SocketSender.StartClient(message.fullHL7Message, RegistryIp, RegistryPort);
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.Error, "Could not open socket to the regisry : " + e.Message);
                    Console.WriteLine("Could not open socket to the regisry : " + e.Message);
                    return;
                }

                response = hl7h.HandleResponse(ret);

                if (ret.Contains("SOA"))
                {                    
                    if (response.segments[0].fields[1] != "OK")
                    {
                        Console.WriteLine("Could not validate team");
                        
                        logger.Log(LogLevel.Error, "Could not validate team");
                        response = hl7h.HandleResponse(ret);
                        LogUtility.logMessage(response);
                        
                        Service errorResponse = new Service();
                        errorResponse.errorCode = "-4";
                        errorResponse.errorMessage = "Could not validate team";

                        message = hl7h.BuildResponseMessage(errorResponse, true);
                        Send(handler, message.fullHL7Message);                        
                        return;
                    }
                    else
                    {
                        //create message, return it
                        string province;
                        string principle;

                        if (clientData.segments[2].fields[1] == "1")
                        {
                            province = clientData.segments[2].fields[5];
                            principle = clientData.segments[3].fields[5];
                        }
                        else
                        {
                            province = clientData.segments[3].fields[5];
                            principle = clientData.segments[2].fields[5];
                        }

                        string testProvince = province;
                        province = province.ToUpper();                        

                        double p = 0;
                        try {
                            p = double.Parse(principle);
                        }
                        catch (Exception e)
                        {
                            logger.Log(LogLevel.Error, "Principal " + principle + " is not a valid value");
                            Service errorResponse = new Service();
                            errorResponse.errorCode = "-4";
                            errorResponse.errorMessage = "Principal " + principle + " is not a valid value";

                            message = hl7h.BuildResponseMessage(errorResponse, true);
                            Send(handler, message.fullHL7Message);
                            return;
                        }

                        PurchaseTotaller pt = new PurchaseTotaller(province, p);
                        if (pt.province == -1)
                        {
                            logger.Log(LogLevel.Error, "Province " + testProvince + " could not be found");
                            Service errorResponse = new Service();
                            errorResponse.errorCode = "-4";
                            errorResponse.errorMessage = "Province " + testProvince + " is not valid";

                            HL7 errorhl7 = hl7h.BuildResponseMessage(errorResponse, true);
                            Send(handler, errorhl7.fullHL7Message);
                            return;
                        }

                        pt.AddResult(1, pt.responses[0].Name, pt.responses[0].DataType, pt.principal);
                        pt.AddResult(2, pt.responses[1].Name, pt.responses[1].DataType, pt.GetPST());
                        pt.AddResult(3, pt.responses[2].Name, pt.responses[2].DataType, pt.GetHST());
                        pt.AddResult(4, pt.responses[3].Name, pt.responses[3].DataType, pt.GetGST());
                        pt.AddResult(5, pt.responses[4].Name, pt.responses[4].DataType, pt.GetTotal());

                        Service purchaseTotalResults = new Service();
                        purchaseTotalResults.Responses = pt.results;

                        message = hl7h.BuildResponseMessage(purchaseTotalResults);                    
                        Send(handler, message.fullHL7Message);
                        
                    }   
                }
                else
                {
                    logger.Log(LogLevel.Error, "Invalid response from registry");
                    Console.WriteLine("Invalid response from registry");
                    return;
                }
            }
        }

        private void Send(Socket handler, String data)
        {
            logger.Log(LogLevel.Info, "Responding to service request :\n");
            logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(data));
            logger.Log(LogLevel.Info, "---\n");
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Could not send response to client : " + e.Message);
                Console.WriteLine("Could not send response to client.");
                return;
            }
        }

        public void StopTheRegistryFromTrolling()
        {
            string REGISTRY_IP = ConfigurationManager.AppSettings["RegistryIP"];
            string REGISTRY_PORT = ConfigurationManager.AppSettings["RegistryPort"];
            string SERVICE_NAME = ConfigurationManager.AppSettings["ServiceName"];
            string SERVICE_IP = ConfigurationManager.AppSettings["ServiceIP"];
            string SERVICE_PORT = ConfigurationManager.AppSettings["ServicePort"];
            HL7Handler hl7h = new HL7Handler();
            int RegistryPort = 0;
            int Port = 0;

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

            try
            {
                int.TryParse(SERVICE_PORT, out Port);
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, "Invalid service port read from config file: " + e.Message);
                Console.WriteLine("Invalid service port read from config file. Exiting.");
                return;
            }

            //every 30 seconds, try to register the team, and publish the service
            while (true)
            {
                System.Threading.Thread.Sleep(1000);

                //
                //Register team 
                //
                Service register = new Service();
                register.TeamName = TEAM_NAME;

                message = hl7h.RegisterTeamMessage(register);
                LogUtility.logMessage(message);

                string ret = "";
                try
                {
                    ret = SocketSender.StartClient(message.fullHL7Message, REGISTRY_IP, RegistryPort);
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.Error, "Could not open socket to the regisry : " + e.Message);
                    Console.WriteLine("Could not open socket to the regisry : " + e.Message);
                    return;
                }

                //
                //Publish team
                //
                PurchaseTotaller pt = new PurchaseTotaller();
                IPAddress ipAddress = IPAddress.Parse(SERVICE_IP);
                Service service = new Service(SERVICE_NAME, TEAM_NAME, TEAM_ID, PurchaseTotaller.TAG_NAME, PurchaseTotaller.SECURITY_LEVEL, PurchaseTotaller.DESCRIPTION, pt.arguments, pt.responses, ipAddress, Port);

                logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
                message = hl7h.PublishServiceMessage(service);
                LogUtility.logMessage(message);

                try
                {
                    ret = SocketSender.StartClient(message.fullHL7Message, REGISTRY_IP, RegistryPort);
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.Error, "Could not open socket to the regisry : " + e.Message);
                    Console.WriteLine("Could not open socket to the regisry : " + e.Message);
                    return;
                }

            }
        }



    }

   

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}




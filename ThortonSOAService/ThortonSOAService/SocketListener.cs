﻿using System;
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
            string command = hl7h.RegisterTeamMessage(team);
            logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(command));
            string ret = SocketSender.StartClient(command, RegistryIp, RegistryPort);
            logger.Log(LogLevel.Info, "\t>> Response from Registry:\n");
            logger.Log(LogLevel.Info, "\t\t>> " + HL7Parser.LogSegment(ret));

            if (ret.Contains("SOA"))
            {
                HL7 hl7 = hl7h.HandleResponse(ret);

                if (hl7.segments[0].fields[1] != "OK")
                {
                    logger.Log(LogLevel.Error, "Could not retrieve team id");
                    Console.WriteLine("Could not retrieve team id");
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
                HL7 record = hl7h.HandleResponse(content);

                //take team info    
                //query registry           
                Service service = new Service(TEAM_NAME, TEAM_ID);
                Service teamService = new Service(record.segments[0].fields[2], record.segments[0].fields[3]);
                teamService.Tag = ConfigurationManager.AppSettings["TagName"];
                string command = hl7h.QueryTeamMessage(service, teamService);
                string ret = SocketSender.StartClient(command, RegistryIp, RegistryPort);

                if (ret.Contains("SOA"))
                {
                    HL7 hl7 = hl7h.HandleResponse(ret);

                    if (hl7.segments[0].fields[1] != "OK")
                    {
                        logger.Log(LogLevel.Error, "Could not validate team");
                        Console.WriteLine("Could not validate team");
                        return;
                    }
                    else
                    {
                        //create message, return it
                        string province;
                        string principle;

                        if (record.segments[2].fields[1] == "1")
                        {
                            province = record.segments[2].fields[5];
                            principle = record.segments[3].fields[5];
                        }
                        else
                        {
                            province = record.segments[3].fields[5];
                            principle = record.segments[2].fields[5];
                        }

                        PurchaseTotaller pt = new PurchaseTotaller(province, principle);
                        pt.AddResult(1, pt.responses[0].Name, pt.responses[0].DataType, pt.principal);
                        pt.AddResult(2, pt.responses[1].Name, pt.responses[1].DataType, pt.GetPST());
                        pt.AddResult(3, pt.responses[2].Name, pt.responses[2].DataType, pt.GetHST());
                        pt.AddResult(4, pt.responses[3].Name, pt.responses[3].DataType, pt.GetGST());
                        pt.AddResult(5, pt.responses[4].Name, pt.responses[4].DataType, pt.GetTotal());

                        Service response = new Service();
                        response.Responses = pt.results;

                        ret = hl7h.BuildResponseMessage(response);
                        Send(handler, ret);
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




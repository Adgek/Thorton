﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HL7Records;
using SocketClass;
using NLog;
using System.Configuration;

namespace ThortonSOAService
{
    class SocketListener
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public const int PORT = 11000;
        public string TEAM_ID;
        public string TEAM_NAME;
        public string SERVICE_TAG;

        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public SocketListener()
        {
            TEAM_NAME = "FunnyGlasses";

            //QUERY FOR TEAM ID
            HL7Handler hl7h = new HL7Handler();
            logger.Log(LogLevel.Info, "Calling SOA-Registry with message :\n");
            string command = hl7h.RegisterTeamMessage();
            string ret = SocketSender.StartClient(command);

            HL7 hl7 = hl7h.HandleResponse(ret);
            if (hl7.segments[0].fields[1] != "OK")
            {
                //throw error
            }
            TEAM_ID = hl7.segments[0].fields[2];
            logger.Log(LogLevel.Info, "---\n");

            SERVICE_TAG = ConfigurationManager.AppSettings["purchaseTotallerTag"];
        }

        public void StartListening()
        { 
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.   
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
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
                    listener.BeginAccept( new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                
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

                //read content               
                HL7Handler hl7h = new HL7Handler();
                HL7 record = hl7h.HandleResponse(content);
                //take team info               
                Service service = new Service(TEAM_NAME, TEAM_ID);
                Service teamService = new Service("", "");

                string command = hl7h.QueryTeamMessage(service);
                string ret = SocketSender.StartClient(command);

                HL7 hl7 = hl7h.HandleResponse(ret);
                //query registry
                //if team is valid {

                PurchaseTotaller pt = new PurchaseTotaller("ON", "100.00");
                pt.AddResult(1, pt.responses[0].ResponseName, pt.responses[0].ResponseDataType, pt.principal);
                pt.AddResult(2, pt.responses[1].ResponseName, pt.responses[1].ResponseDataType, pt.GetPST());
                pt.AddResult(3, pt.responses[2].ResponseName, pt.responses[2].ResponseDataType, pt.GetHST());
                pt.AddResult(4, pt.responses[3].ResponseName, pt.responses[3].ResponseDataType, pt.GetGST());
                pt.AddResult(5, pt.responses[4].ResponseName, pt.responses[4].ResponseDataType, pt.GetTotal());

                //create message, return it


            }
        }

        private void Send(Socket handler, String data)
        {
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
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
               
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




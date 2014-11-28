﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ThortonSOAService
{
    class SocketSender
    {
        private const int PORT = 3128;
        private const string IP_ADDRESS = "192.168.2.32";

        public static string StartClient(string message)
        {
            string result = "";
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];
            
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPAddress ipAddress = IPAddress.Parse(IP_ADDRESS);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, PORT);                

                // Create a TCP/IP  socket.
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    result = Encoding.ASCII.GetString(bytes);



                }
                catch (ArgumentNullException ane)
                {
                    
                }
                catch (SocketException se)
                {
                   
                }
                catch (Exception e)
                {
                    
                }
            }
            catch (Exception e)
            {
                
            }

            return result;
        }
    }
}

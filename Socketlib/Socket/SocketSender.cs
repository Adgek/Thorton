//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: SocketSender.cs
//Date: 30/11/14
//Purpose: this file contains the class to handle syncronous socket communication mainly used for service/client to registry communications
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SocketClass
{
    /// <summary>
    /// Socket class to open s syncronous socket connection and return the response
    /// </summary>
    static public class SocketSender
    {
        private static int PORT = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        private static string IP_ADDRESS = ConfigurationManager.AppSettings["ipaddress"];

        /// <summary>
        /// send a message through a socket and return the response
        /// </summary>
        /// <param name="message">data to send</param>
        /// <param name="ip">ip to open the socket</param>
        /// <param name="port">port to open the socket</param>
        /// <returns>string response</returns>
        public static string StartClient(string message, string ip, int port)
        {
            IP_ADDRESS = ip;
            PORT = port;
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
                catch (Exception e)
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
    }
}

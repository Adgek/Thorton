//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: Service.cs
//Date: 23/11/14
//Purpose: This file contains the Service object in our application. The service object is an object that contains all the data
//          that a specific service requires to work.
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    /// <summary>
    /// Need a function to check if amount of fields equals what was expected, if not then segment is not valid.
    /// </summary>
    /// <param name="numFields">This is the protocols number of fields to check if segment is valid</param>
    public class Service
    {
        //Service data members
        public string ServiceName;
        public string TeamName;
        public string TeamID;
        public string Tag;
        public int SecurityLevel;
        public string Description;
        public string errorCode;
        public string errorMessage;

        public List<Message> Arguments;
        public List<Message> Responses;

        public IPAddress IP;
        public int Port;


        /// <summary>
        /// default constructor
        /// </summary>
        public Service()
        {
            Arguments = new List<Message>();
            Responses = new List<Message>();                      
        }


        /// <summary>
        /// This constructor only needs a team name and ID
        /// </summary>
        /// <param name="name">This is the name of the team</param>
        /// <param name="id">This is the team ID</param>
        public Service(string name, string id) : this()
        {
            TeamName = name;
            TeamID = id;
        }


        /// <summary>
        /// This constructor only needs a team name ID, and service tag
        /// </summary>
        /// <param name="name">This is the name of the team</param>
        /// <param name="id">This is the team ID</param>
        /// <param name="tagName">This is the service tag name</param>
        public Service(string name, string id, string tagName)
            : this(name, id)
        {
            Tag = tagName;
        }


        /// <summary>
        /// This constructor takes all the datamembers
        /// </summary>
        /// <param name="sname">This is the name of the service</param>
        /// <param name="name">This is the name of the team</param>
        /// <param name="id">This is the team ID</param>
        /// <param name="tag">This is the service tag name</param>
        /// <param name="sec">This is the service security level</param>
        /// <param name="desc">This is the service description</param>
        /// <param name="args">This is the service's arguments</param>
        /// <param name="resps">This is the service's responses</param>
        /// <param name="ip">This is the IP that the service lives at</param>
        /// <param name="port">This is the Port number that the service listens on</param>
        public Service(string sname, string name, string id, string tag, int sec, string desc, List<Message> args, List<Message> resps, IPAddress ip, int port)
        {
            Arguments = args;
            Responses = resps;
                        
            ServiceName = sname;
            TeamName = name;
            TeamID = id;
            Tag = tag;
            SecurityLevel = sec;
            Description = desc;
            IP = ip;
            Port = port;
        }  
    }
}

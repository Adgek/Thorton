using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    public class Service
    {
        public string ServiceName;
        public string TeamName;
        public string TeamID;
        public string Tag;
        public int SecurityLevel;
        public string Description;

        public List<Argument> Arguments;
        public List<Response> Responses;

        public IPAddress IP;
        public int Port;

        public Service()
        {
            Arguments = new List<Argument>();
            Responses = new List<Response>();                      
        }

        public Service(string name, string id) : this()
        {
            TeamName = name;
            TeamID = id;
        }

        public Service(string sname, string name, string id, string tag, int sec, string desc, List<Argument> args, List<Response> resps, IPAddress ip, int port)
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

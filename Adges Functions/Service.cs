using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace HL7Records
{
    class Service
    {
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

            TeamName = "";
            TeamID = "";
            Tag = "";
            SecurityLevel = 0;
            Description = "";
            IP = null;
            Port = 0;
        }        
    }

    class Argument
    {
        public int Position;
        public string ArgumentName;
        public string ArgumentDataType;
        public bool Mandatory;

        public Argument()
        {
            Position = 0;
            ArgumentName = "";
            ArgumentDataType = "";
            Mandatory = true;
        }
    }

    class Response
    {
        public int Position;
        public string ResponseName;
        public string ResponseDataType;

        public Response()
        {
            Position = 0;
            ResponseName = "";
            ResponseDataType = "";
        }
    }
}

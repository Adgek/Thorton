using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThortonSOAService
{
    class Service
    {
        private string TeamName;
        private string TeamID;
        private string Tag;
        private int SecurityLevel;
        private string Description;

        private List<Argument> Arguments;
        private List<Response> Responses;

        private IPAddress IP;
        private int Port;

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
        private int Position;
        private string ArgumentName;
        private string ArgumentDataType;
        private bool Mandatory;

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
        private int Position;
        private string ResponseName;
        private string ResponseDataType;

        public Response()
        {
            Position = 0;
            ResponseName = "";
            ResponseDataType = "";
        }
    }
}

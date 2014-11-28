using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    public class Response
    {
        public int Position;
        public string ResponseName;
        public string ResponseDataType;
        public string value;

        public Response()
        {
            Position = 0;
            ResponseName = "";
            ResponseDataType = "";
            value = "";
        }

        public Response(int pos, string name, string datatype, string val = "")
        {
            Position = pos;
            ResponseName = name;
            ResponseDataType = datatype;
            value = val;
        }
    }
}

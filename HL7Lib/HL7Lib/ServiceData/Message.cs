using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    public class Message
    {
        public int Position;
        public string Name;
        public string DataType;
        public bool Mandatory;
        public string Value;

        public Message()
        {
            Position = 0;
            Name = "";
            DataType = "";
            Mandatory = true;
        }

        public Message(int pos, string name, string datatype, bool man, string value = "")
        {
            Position = pos;
            Name = name;
            DataType = datatype;
            Mandatory = man;
            Value = value;
        }
    }
}

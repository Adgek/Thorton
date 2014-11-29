using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    public class Argument
    {
        public int Position;
        public string ArgumentName;
        public string ArgumentDataType;
        public bool Mandatory;
        public string Value;

        public Argument()
        {
            Position = 0;
            ArgumentName = "";
            ArgumentDataType = "";
            Mandatory = true;
        }

        public Argument(int pos, string name, string datatype, bool man, string value = "")
        {
            Position = pos;
            ArgumentName = name;
            ArgumentDataType = datatype;
            Mandatory = man;
            Value = value;
        }
    }
}

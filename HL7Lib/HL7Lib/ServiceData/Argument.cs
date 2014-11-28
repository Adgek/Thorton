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

        public Argument()
        {
            Position = 0;
            ArgumentName = "";
            ArgumentDataType = "";
            Mandatory = true;
        }

        public Argument(int pos, string name, string datatype, bool man)
        {
            Position = pos;
            ArgumentName = name;
            ArgumentDataType = datatype;
            Mandatory = man;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.HL7
{
    static class Definitions
    {
        const char BEGIN_MESSAGE = (char) 11;
        const char END_SEGMENT = (char)13;
        const char END_MESSAGE = (char)28;

        public const int SOA_NUM_FIELDS = 5;
        public const int DRC_NUM_FIELDS = 4;
        public const int INF_NUM_FIELDS = 4;
        public const int SRV_NUM_FIELDS = 7;
        public const int ARG_NUM_FIELDS = 6;
        public const int RSP_NUM_FIELDS = 5;
        public const int MCH_NUM_FIELDS = 3;
        public const int PUB_NUM_FIELDS = 5;        
    }
}

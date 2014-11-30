using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.HL7
{
    static class Definitions
    {
        enum MessageTypes
        {

        }

        const char BEGIN_MESSAGE = (char) 11;
        const char END_SEGMENT = (char)13;
        const char END_MESSAGE = (char)28;

        public static const int SOA_NUM_FIELDS = 5;
        public static const int DRC_NUM_FIELDS = 4;
        public static const int INF_NUM_FIELDS = 4;
        public static const int SRV_NUM_FIELDS = 7;
        public static const int ARG_NUM_FIELDS = 6;
        public static const int RSP_NUM_FIELDS = 5;
        public static const int MCH_NUM_FIELDS = 3;
        public static const int PUB_NUM_FIELDS = 5;
        
    }
}

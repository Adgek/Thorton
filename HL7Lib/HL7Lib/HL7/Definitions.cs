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

        const int DRC_NUM_FIELDS = 4;
        const int INF_NUM_FIELDS = 4;
        const int SRV_NUM_FIELDS = 7;
        const int ARG_NUM_FIELDS = 6;
        const int RSP_NUM_FIELDS = 5;
        const int MCH_NUM_FIELDS = 3;
        const int PUB_NUM_FIELDS = 5;
        
    }
}

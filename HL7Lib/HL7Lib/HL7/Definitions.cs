//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: Definitions.cs
//Date: 23/11/14
//Purpose: This file contains the definitions of specific HL7 protocol variables
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.HL7
{
    /// <summary>
    /// This is the static consts definitions needed for HL7
    /// </summary>
    static class Definitions
    {
        //Define ascii chars needed for HL7 protocol
        public const char BEGIN_MESSAGE = (char) 11;
        public const char END_SEGMENT = (char)13;
        public const char END_MESSAGE = (char)28;

        //Define the length of HL7 segment types
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

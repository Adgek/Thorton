//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: Message.cs
//Date: 23/11/14
//Purpose: This file contains the definition of a message. A message is either an argument or response HL7 segment
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Lib.ServiceData
{
    /// <summary>
    /// Constructor takes an HL7Segment string for responses
    /// </summary>
    public class Message
    {
        public int Position;
        public string Name;
        public string DataType;
        public bool Mandatory;
        public string Value;

        /// <summary>
        /// blank constructor
        /// </summary>
        public Message()
        {
            Position = 0;
            Name = "";
            DataType = "";
            Mandatory = true;
        }

        /// <summary>
        /// Constructor takes all fields 
        /// </summary>
        /// <param name="pos">This is the position number of the message</param>
        /// <param name="name">This is the name of the message</param>
        /// <param name="datatype">This is the datatype of the message</param>
        /// <param name="man">This is the variable determining if the message is mandatory, it is Optional</param>
        /// <param name="value">This is the value of the message, it is Optional</param>
        public Message(int pos, string name, string datatype, bool man = true, string value = "")
        {
            Position = pos;
            Name = name;
            DataType = datatype;
            Mandatory = man;
            Value = value;
        }
    }
}

//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: HL7Parser.cs
//Date: 23/11/14
//Purpose: This file contains the static parser class that contains methods for parsing segments.
//***********************

using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7Lib.HL7
{
	/// <summary>
    /// This is where the HL7 parsing logic goes.
    /// </summary>
	public static class HL7Parser
	{

		/// <summary>
        /// Need a function for pulling segments from a message
        /// </summary>
        /// <param name="message">This is the message that will be parsed for segments</param>
		public static List<string> GetSegmentsFromMessage(string message)
		{	
			//remove char11 from beggining
			message = message.Replace(Definitions.BEGIN_MESSAGE.ToString(), "");
			//remove char28, char13 from end
			message = message.Replace(Definitions.END_MESSAGE.ToString()+Definitions.END_SEGMENT.ToString(), "");
			//string.split on char 13
			List<string> returnList = new List<string>(message.Split(Definitions.END_SEGMENT));

			//remove last empty field from list.
			returnList.RemoveAt(returnList.Count - 1);
			return returnList;
		}
		
		/// <summary>
        /// Need a function for pulling list of string from a segment
        /// </summary>
        /// <param name="segment">This is the segment that will be parsed for fields</param>
		public static List<string> GetFieldsFromSegment(string segment)
		{
			List<string> returnList = new List<string>(segment.Split('|'));
			//remove last empty field from list.
			returnList.RemoveAt(returnList.Count - 1);
			return returnList;
		}


		/// <summary>
        /// Need a function for getting a segment ready for logging
        /// </summary>
        /// <param name="message">This is the segment that will be prepared</param>
        public static string LogSegment(string message)
        {
            message = message.Replace(Definitions.BEGIN_MESSAGE.ToString(), "");
            message = message.Replace(Definitions.END_MESSAGE.ToString() + Definitions.END_SEGMENT.ToString(), "");
            message = message.Replace(Definitions.END_SEGMENT.ToString(), "--");

            return message;
        }

	}
}
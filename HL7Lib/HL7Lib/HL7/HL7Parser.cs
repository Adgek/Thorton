using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7Lib.HL7
{
	public static class HL7Parser
	{
		//This is where the parsing logic goes.
			//This could maybe be a static class.

		//Need a function for pulling segments from a message
		public static List<string> GetSegmentsFromMessage(string message)
		{	
			char msgBeg = (char)11;
			char segEnd = (char)13;
			char msgEnd = (char)28;
			//remove char11 from beggining
			//remove char28, char13 from end
			message = message.Replace(msgBeg.ToString(), "");
			message = message.Replace(msgEnd.ToString()+segEnd.ToString(), "");
			//message = message.Trim(new char[] {msgBeg, msgEnd.ToString()+seg.ToString()});
			//string.split on char 13
			List<string> returnList = new List<string>(message.Split(segEnd));

			//remove last empty field from list.
			returnList.RemoveAt(returnList.Count - 1);
			return returnList;
		}
		
		//Need a function for pulling list of string from a segment
		public static List<string> GetFieldsFromSegment(string segment)
		{
			List<string> returnList = new List<string>(segment.Split('|'));
			//remove last empty field from list.
			returnList.RemoveAt(returnList.Count - 1);
			return returnList;
		}

        public static string LogSegment(string message)
        {
            char msgBeg = (char)11;
			char segEnd = (char)13;
			char msgEnd = (char)28;
            
            message = message.Replace(msgBeg.ToString(), "");
            message = message.Replace(msgEnd.ToString() + segEnd.ToString(), "");
            message = message.Replace(segEnd.ToString(), "\n");

            return message;
        }

	}
}
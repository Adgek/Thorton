using System;
using System.Collections.Generic;

namespace HL7Lib.HL7
{
    public class HL7
	{
		//This is the HL7 object for easy transport of its datamembers

		//DEFINES
		//public static const BEGGINING_MESSAGE_CHAR = '\u11';
		//public static const END_SEGMENT_CHAR = '\u11';
		//public static const END_MESSAGE_CHAR = '\u11';


		//This is the list of segments that make up a HL7
		public List<HL7Segment> segments;

		public string fullHL7Message { get; set;}

		//Constructor is blank for the HL7 builder
		public HL7()
		{
			segments = new List<HL7Segment>();
		}

        public string Validate()
        {
            string returnVal = "The following segments do not match the HL7 protocol: " + Environment.NewLine;
            Boolean isValid = true;
            foreach(HL7Segment seg in segments)
            {
                if(!seg.isValid)
                {
                    isValid = false;
                    returnVal += seg.segment + Environment.NewLine;
                }
            }
            if (isValid)
                return "valid";
            return returnVal;
        }
		
		//Constructor takes an HL7 string for responses
		public HL7(string message):this()
		{
			fullHL7Message = message;

			List<string> parsedSegments = HL7Parser.GetSegmentsFromMessage(fullHL7Message);
			
			//poplulate segments list
			foreach (string segment in parsedSegments)
			{
				HL7Segment newSegment = new HL7Segment(segment, 2); 
				segments.Add(newSegment);
			}
		}


	}
}
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
		
		//Constructor takes an HL7 string for responses
		public HL7(string message):this()
		{
			fullHL7Message = message;

			List<string> parsedSegments = HL7Parser.GetSegmentsFromMessage(fullHL7Message);

            int numFields = 0;

			//poplulate segments list
			foreach (string segment in parsedSegments)
			{
                string segType = segment.Substring(0, 3);
                if (segType.Contains("SOA"))
                {
                    numFields = Definitions.SOA_NUM_FIELDS;
                }
                else if (segType.Contains("DRC"))
                {
                    numFields = Definitions.DRC_NUM_FIELDS;
                }
                else if (segType.Contains("INF"))
                {
                    numFields = Definitions.INF_NUM_FIELDS;
                }
                else if (segType.Contains("SRV"))
                {
                    numFields = Definitions.SRV_NUM_FIELDS;
                }
                else if (segType.Contains("ARG"))
                {
                    numFields = Definitions.ARG_NUM_FIELDS;
                }
                else if (segType.Contains("RSP"))
                {
                    numFields = Definitions.RSP_NUM_FIELDS;
                }
                else if (segType.Contains("MCH"))
                {
                    numFields = Definitions.MCH_NUM_FIELDS;
                }
                else if (segType.Contains("PUB"))
                {
                    numFields = Definitions.PUB_NUM_FIELDS;
                }

				HL7Segment newSegment = new HL7Segment(segment, numFields); 
				segments.Add(newSegment);
			}
		}


	}
}
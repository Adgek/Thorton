//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: HL7.cs
//Date: 23/11/14
//Purpose: This file contains the definition of what makes a HL7 object. it contains a list of Segments
//          that make up an HL7 message.
//***********************

using System;
using System.Collections.Generic;

namespace HL7Lib.HL7
{
    /// <summary>
    /// This is the HL7 object for easy transport of its datamembers
    /// </summary>
    public class HL7
	{

		//This is the list of segments that make up a HL7
		public List<HL7Segment> segments;

        //Full string of HL7Message
		public string fullHL7Message { get; set;}

        /// <summary>
        /// Constructor is blank for the HL7 builder
        /// </summary>
		public HL7()
		{
			segments = new List<HL7Segment>();
		}
		
        /// <summary>
        /// Constructor takes an HL7 string for responses
        /// </summary>
        /// <param name="message">message to parse into HL7</param>
		public HL7(string message):this()
		{
			fullHL7Message = message;

			List<string> parsedSegments = HL7Parser.GetSegmentsFromMessage(fullHL7Message);

            int numFields = 0;

			//poplulate segments list
			foreach (string segment in parsedSegments)
			{
                //get validate number of fields for each type of segment
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

        /// <summary>
        /// Validate all segments in an HL7 message
        /// </summary>
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


	}
}
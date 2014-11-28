using System;
using System.Collections.Generic;

namespace HL7Records
{
	class HL7Segment
	{
		//This is the HL7Segment object

		//This List is a list of fields in the segment
		public List<string> fields;
		public string segment { get; set;}

		//Constructor is blank for the HL7Segment builder
		public HL7Segment()
		{
			fields = new List<string>();
		}
		
		//Constructor takes an HL7Segment string for responses
		public HL7Segment(string fullSegment)
		{
			segment = fullSegment;

			fields = HL7Parser.GetFieldsFromSegment(fullSegment);
		}

		public void ConvertFieldsToSegmentString()
		{
			foreach (string field in fields)
			{
				segment+=field + "|";
			}
		}

		//Maybe have a validate segment method that will check fields.count agianst 
		//passed in value to determine number of fields


	}
}
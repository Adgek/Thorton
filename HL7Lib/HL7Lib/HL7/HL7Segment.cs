using System;
using System.Collections.Generic;

namespace HL7Lib.HL7
{
    public class HL7Segment
	{
		//This is the HL7Segment object

		//This List is a list of fields in the segment
		public List<string> fields;
		public string segment { get; set;}
        public bool isValid { get; set; }
		//Constructor is blank for the HL7Segment builder
		public HL7Segment()
		{
			fields = new List<string>();
            isValid = true;
		}
		
		//Constructor takes an HL7Segment string for responses
		public HL7Segment(string fullSegment, int numFields)
		{
            isValid = true;
			segment = fullSegment;

			fields = HL7Parser.GetFieldsFromSegment(fullSegment);
            
            isSegmentValid(numFields);
		}

		public void ConvertFieldsToSegmentString()
		{
			foreach (string field in fields)
			{
				segment+=field + "|";
			}
		}

        public void isSegmentValid(int numFields)
        {
            if (fields.Count != numFields)
            {
                isValid = false;
            }
        }
		//Maybe have a validate segment method that will check fields.count agianst 
		//passed in value to determine number of fields


	}
}
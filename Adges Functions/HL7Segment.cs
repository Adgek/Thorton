using System;
using System.Collections.Generic;



namespace test
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
			
		}
		
		//Constructor takes an HL7Segment string for responses
		public HL7Segment(string fullSegment)
		{
			segment = fullSegment;
			fields = new List<string>();
		}


	}
}
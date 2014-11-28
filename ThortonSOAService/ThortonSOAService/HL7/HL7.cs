using System;
using System.Collections.Generic;



namespace HL7Records
{
	class HL7
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
			
		}


	}
}
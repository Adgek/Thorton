using System;
using System.Collections.Generic;



namespace test
{
	class HL7
	{
		//This is the HL7 object for easy transport of its datamembers

		//This is the list of segments that make up a HL7
		public List<HL7Segment> segments;

		public string testValue { get; set;}

		//Constructor is blank for the HL7 builder
		public HL7()
		{
			testValue = "yooyo";
		}
		
		//Constructor takes an HL7 string for responses
		public HL7(string message)
		{
			
		}


	}
}
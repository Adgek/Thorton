using System;
using System.Collections.Generic;



namespace test
{
	class HL7Builder
	{
		//This is the HL7Builder which creates the different messages to send.

		//Needs a method for building a Register Team message
		public static HL7 BuildRegisterTeamMessage()
		{
			HL7 builtHL7 = new HL7();
			string cmd = "";
			string teamName = "";

			//create DRC segment
			cmd = "REG-TEAM";
			BuildDRCSegment(builtHL7,cmd);

			//create INF segment
			teamName = "FunnyGlasses";
			BuildINFSegment(builtHL7,teamName);
			//create full string
				//
			return builtHL7; 
		}

		//Needs a method for building a Unregister Team message
		public static HL7 BuildUnregisterTeamMessage()
		{
			HL7 builtHL7 = new HL7();

			return builtHL7; 
		}
		//Needs a method for building a Query Team message
		public static HL7 BuildQueryTeamMessage()
		{
			HL7 builtHL7 = new HL7();

			return builtHL7; 
		}
		//Needs a method for building a Publish Service message
		public static HL7 BuildPublishServiceMessage()
		{
			HL7 builtHL7 = new HL7();

			return builtHL7; 
		}
		//Needs a method for building a Query Service message
		public static HL7 BuildQueryServiceMessage()
		{
			HL7 builtHL7 = new HL7();

			return builtHL7; 
		}
		//Needs a method for building a Execute Service message
		public static HL7 BuildExecuteServiceMessage()
		{
			HL7 builtHL7 = new HL7();

			return builtHL7; 
		}

		//Need method for creating DRC segment
		public static void BuildDRCSegment(HL7 builtHL7, string cmd, string teamName="", string teamID="")
		{
			string fullSegment = "";
			string segmentTitle = "DRC";
			
			fullSegment = "this is a segment";
			HL7Segment newSegment = new HL7Segment(fullSegment);
			builtHL7.segments.Add(newSegment);

			//builtHL7.segments.Add(new HL7Segment(fullSegment));
			//might just return void and edit the HL7 or return string	
		}
		//Need method for creating INF segment
		public static void BuildINFSegment(HL7 builtHL7, string teamName, string teamID="", string serviceTagName="")
		{
			string segmentTitle = "INF";
			//might just return void and edit the HL7 or return string	
		}
		//Need method for creating SRV segment
		public static void BuildSRVSegment(HL7 builtHL7)
		{
			//might just return void and edit the HL7 or return string	
		}
		//Need method for creating ARG segment
		public static void BuildARGSegment(HL7 builtHL7)
		{
			//might just return void and edit the HL7 or return string	
		}
		//Need method for creating RSP segment
		public static void BuildRSPSegment(HL7 builtHL7)
		{
			//might just return void and edit the HL7 or return string	
		}
		//Need method for creating MCH segment
		public static void BuildMCHSegment(HL7 builtHL7)
		{
			//might just return void and edit the HL7 or return string	
		}



	}
}
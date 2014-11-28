using System;
using System.Collections.Generic;



namespace HL7Records
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
			FinalizeHL7Protocol(builtHL7);
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
			string segmentTitle = "DRC";
			
			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(cmd);
			newSegment.fields.Add(teamName);
			newSegment.fields.Add(teamID);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);
	
		}

		//Need method for creating INF segment
		public static void BuildINFSegment(HL7 builtHL7, string teamName, string teamID="", string serviceTagName="")
		{
			string segmentTitle = "INF";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(teamName);
			newSegment.fields.Add(teamID);
			newSegment.fields.Add(serviceTagName);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);
				
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

		//Add on HL7 specific chars
		public static void FinalizeHL7Protocol(HL7 builtHL7)
		{
			char msgBeg = (char)11;
			char segEnd = (char)13;
			char msgEnd = (char)28;//Put these as CONSTS in HL7 class

			string messagebuilder = msgBeg.ToString();

			foreach(HL7Segment seg in builtHL7.segments)
			{
				messagebuilder+= seg.segment + segEnd.ToString();

			}

			messagebuilder += msgEnd.ToString() + segEnd.ToString();
			builtHL7.fullHL7Message = messagebuilder;
		}


	}
}
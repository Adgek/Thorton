using System;
using System.Collections.Generic;
using System.Net;

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
		public static HL7 BuildQueryTeamMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "QUERY-TEAM"; 
			
			//create DRC
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create INF
			BuildINFSegment(builtHL7, myService.TeamName, myService.TeamID, myService.Tag);

			FinalizeHL7Protocol(builtHL7);
			return builtHL7; 
		}
		
		//Needs a method for building a Publish Service message
		public static HL7 BuildPublishServiceMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "PUB-SERVICE";
			int argsNum = myService.Arguments.Count;
			int respNum = myService.Responses.Count;
			

			//create DRC segment
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create SRV
			//TMP
			string serviceName = myService.ServiceName;
			
			BuildSRVSegment(builtHL7, myService.Tag, serviceName, myService.SecurityLevel.ToString(), argsNum.ToString(), respNum.ToString(), myService.Description);

			//create ARG's

			foreach (Argument arg in myService.Arguments)
			{
				BuildARGSegment(builtHL7, arg.Position.ToString(), arg.ArgumentName, arg.ArgumentDataType, arg.Mandatory);
			}

			
			
			//create RSP
			foreach (Response resp in myService.Responses)
			{
				BuildRSPSegment(builtHL7, resp.Position.ToString(), resp.ResponseName, resp.ResponseDataType);
			}
			
			
			//create MCH

			BuildMCHSegment(builtHL7, myService.IP.ToString(), myService.Port.ToString());
			
			FinalizeHL7Protocol(builtHL7);
			return builtHL7; 
		}
		//Needs a method for building a Query Service message
		public static HL7 BuildQueryServiceMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "QUERY-SERVICE";
			
			//create DRC
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create SRV
			
			BuildSRVSegment(builtHL7, myService.Tag);

			FinalizeHL7Protocol(builtHL7);
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
		public static void BuildSRVSegment(HL7 builtHL7, string tagName, string serviceName="", string securityLvl="", string argsNum="", string respNum="", string description="")
		{
			string segmentTitle = "SRV";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(tagName);
			newSegment.fields.Add(serviceName);
			newSegment.fields.Add(securityLvl);
			newSegment.fields.Add(argsNum);
			newSegment.fields.Add(respNum);
			newSegment.fields.Add(description);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);	
		}
		
		//Need method for creating ARG segment
		public static void BuildARGSegment(HL7 builtHL7, string argPos, string argName, string argDataType, bool mandatory)
		{
			string segmentTitle = "ARG";
			string mandatoryString = "mandatory";

			if (mandatory == false)
			{
				mandatoryString = "optional";
			}

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(argPos);
			newSegment.fields.Add(argName);
			newSegment.fields.Add(argDataType);
			newSegment.fields.Add(mandatoryString);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);	
		}
		
		//Need method for creating RSP segment
		public static void BuildRSPSegment(HL7 builtHL7,string respPos, string respName, string respDataType)
		{
			string segmentTitle = "RSP";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(respPos);
			newSegment.fields.Add(respName);
			newSegment.fields.Add(respDataType);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);	
		}
		
		//Need method for creating MCH segment
		public static void BuildMCHSegment(HL7 builtHL7, string IP, string port)
		{
			string segmentTitle = "MCH";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(IP);
			newSegment.fields.Add(port);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);
		}

		//Add on HL7 specific chars
		public static void FinalizeHL7Protocol(HL7 builtHL7)
		{
			char msgBeg = (char)11;
			char segEnd = (char)13;
			char msgEnd = (char)28;

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
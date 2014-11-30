using HL7Lib.ServiceData;
using System;
using System.Collections.Generic;
using System.Net;

namespace HL7Lib.HL7
{
	//This is the HL7Builder which creates the different messages to send.
    public class HL7Builder
	{
		
		//********************************
		//Message Builders
		//********************************

		//Needs a method for building a Register Team message
		public static HL7 BuildRegisterTeamMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "";

			//create DRC segment
			cmd = "REG-TEAM";
			BuildDRCSegment(builtHL7,cmd);

			//create INF segment
			BuildINFSegment(builtHL7,myService.TeamName);
			FinalizeHL7Protocol(builtHL7);
			return builtHL7; 
		}

		//Needs a method for building a Unregister Team message
		public static HL7 BuildUnregisterTeamMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "";

			//create DRC
			cmd = "UNREG-TEAM";
			BuildDRCSegment(builtHL7,cmd,myService.TeamName,myService.TeamID);
			
			FinalizeHL7Protocol(builtHL7);
			return builtHL7; 
		}
		
		//Needs a method for building a Query Team message
		public static HL7 BuildQueryTeamMessage(Service myService, Service queryService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "QUERY-TEAM"; 
			
			//create DRC
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create INF
			BuildINFSegment(builtHL7, queryService.TeamName, queryService.TeamID, queryService.Tag);

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
			string serviceName = "serviceName";
			
			BuildSRVSegment(builtHL7, myService.Tag, serviceName, myService.SecurityLevel.ToString(), argsNum.ToString(), respNum.ToString(), myService.Description);

			//create ARG's
			foreach (Message arg in myService.Arguments)
			{
				BuildARGSegment(builtHL7, arg.Position.ToString(), arg.Name, arg.DataType, arg.Mandatory.ToString());
			}

			//create RSP
			foreach (Message resp in myService.Responses)
			{
				BuildRSPSegment(builtHL7, resp.Position.ToString(), resp.Name, resp.DataType);
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
		public static HL7 BuildExecuteServiceMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "EXEC-SERVICE";
			int argsNum = myService.Arguments.Count;

			//create DRC
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create SRV
			BuildSRVSegment(builtHL7, serviceName: myService.ServiceName, argsNum: argsNum.ToString());

			//create ARG's
			foreach (Message arg in myService.Arguments)
			{
				BuildARGSegment(builtHL7, arg.Position.ToString(), arg.Name, arg.DataType, argValue: arg.Value);
			}

			FinalizeHL7Protocol(builtHL7);
			return builtHL7; 
		}

        public static HL7 BuildServiceResponseOkMessage(Service myService)
        {
            HL7 builtHL7 = new HL7();
            int respNum = myService.Responses.Count;

            //create PUB
            BuildPUBSegment(builtHL7, "OK", numOfSegments: respNum.ToString());

            //create RSP
            foreach (Message resp in myService.Responses)
            {
                BuildRSPSegment(builtHL7, resp.Position.ToString(), resp.Name, resp.DataType, resp.Value);
            }

            FinalizeHL7Protocol(builtHL7);
            return builtHL7; 
            
        }

        public static HL7 BuildServiceResponseNotOkMessage(Service myService)
        {
            HL7 builtHL7 = new HL7();

            //create PUB
            BuildPUBSegment(builtHL7, "NOT-OK", errCode: myService.errorCode, errMessage: myService.errorMessage);

            FinalizeHL7Protocol(builtHL7);
            return builtHL7; 
            
        }



		//********************************
		//Segment Builders
		//********************************

		//Need method for creating DRC segment
		private static void BuildDRCSegment(HL7 builtHL7, string cmd, string teamName="", string teamID="")
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
        private static void BuildINFSegment(HL7 builtHL7, string teamName, string teamID = "", string serviceTagName = "")
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
        private static void BuildSRVSegment(HL7 builtHL7, string tagName = "", string serviceName = "", string securityLvl = "", string argsNum = "", string respNum = "", string description = "")
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
        private static void BuildARGSegment(HL7 builtHL7, string argPos, string argName, string argDataType, string mandatory = "", string argValue = "")
		{
			string segmentTitle = "ARG";
			string mandatoryString = "";

			if (mandatory == "False")
			{
				mandatoryString = "optional";
			}
			if (mandatory == "True")
			{
				mandatoryString = "mandatory";
			}

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(argPos);
			newSegment.fields.Add(argName);
			newSegment.fields.Add(argDataType);
			newSegment.fields.Add(mandatoryString);
			newSegment.fields.Add(argValue);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);	
		}
		
		//Need method for creating RSP segment
        private static void BuildRSPSegment(HL7 builtHL7, string respPos, string respName, string respDataType,string argValue="")
		{
			string segmentTitle = "RSP";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(respPos);
			newSegment.fields.Add(respName);
			newSegment.fields.Add(respDataType);
            newSegment.fields.Add(argValue);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);	
		}
		
		//Need method for creating MCH segment
        private static void BuildMCHSegment(HL7 builtHL7, string IP, string port)
		{
			string segmentTitle = "MCH";

			HL7Segment newSegment = new HL7Segment();
			newSegment.fields.Add(segmentTitle);
			newSegment.fields.Add(IP);
			newSegment.fields.Add(port);
			newSegment.ConvertFieldsToSegmentString();
			builtHL7.segments.Add(newSegment);
		}

        //Need method for creating PUB segment
        private static void BuildPUBSegment(HL7 builtHL7, string result, string errCode="",string errMessage="", string numOfSegments="")
        {
            string segmentTitle = "PUB";

            HL7Segment newSegment = new HL7Segment();
            newSegment.fields.Add(segmentTitle);
            newSegment.fields.Add(result);
            newSegment.fields.Add(errCode);
            newSegment.fields.Add(errMessage);
            newSegment.fields.Add(numOfSegments);
            newSegment.ConvertFieldsToSegmentString();
            builtHL7.segments.Add(newSegment);
        }


		//********************************
		//Utility Functions
		//********************************

		//Add on HL7 specific chars
        private static void FinalizeHL7Protocol(HL7 builtHL7)
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
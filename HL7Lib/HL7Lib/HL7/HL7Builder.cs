//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: SocketListener.cs
//Date: 23/11/14
//Purpose: This file contains the asyncronous service logic which is able to handle multiple clients at the same time.
// based on a microsoft socket example
//***********************

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


		/// <summary>
        /// Needs a method for building a Register Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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


		/// <summary>
        /// Needs a method for building a Unregister Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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
		

		/// <summary>
        /// Needs a method for building a Query Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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
		

		/// <summary>
        /// Needs a method for building a Publish Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public static HL7 BuildPublishServiceMessage(Service myService)
		{
			HL7 builtHL7 = new HL7();
			string cmd = "PUB-SERVICE";
			int argsNum = myService.Arguments.Count;
			int respNum = myService.Responses.Count;
			

			//create DRC segment
			BuildDRCSegment(builtHL7,cmd, myService.TeamName, myService.TeamID);

			//create SRV
			
			BuildSRVSegment(builtHL7, myService.Tag, myService.ServiceName, myService.SecurityLevel.ToString(), argsNum.ToString(), respNum.ToString(), myService.Description);

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
		

		/// <summary>
        /// Needs a method for building a Query Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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
		

		/// <summary>
        /// Needs a method for building a Execute Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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


		/// <summary>
        /// Needs a method for building a Service Response OK message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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


        /// <summary>
        /// Needs a method for building a Service Response NOT OK message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
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
        

        /// <summary>
        /// Need method for creating DRC segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="cmd">HL7 field specifing what the segment does</param>
        /// <param name="teamName">This is the team name, it is Optional</param>
        /// <param name="teamID">This is the team ID, it is Optional</param>
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


		/// <summary>
        /// Need method for creating INF segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="teamName">This is the team name</param>
        /// <param name="teamID">This is the team ID, it is Optional</param>
        /// <param name="serviceTagName">This is the service tag name, it is Optional</param>
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
		

		/// <summary>
        /// Need method for creating SRV segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="tagName">This is the service tag name, it is Optional</param>
        /// <param name="serviceName">This is the service name, it is Optional</param>        
        /// <param name="securityLvl">This is the security level, it is Optional</param>
        /// <param name="argsNum">This is the number of arguments, it is Optional</param>
        /// <param name="respNum">This is the number of responses, it is Optional</param>
        /// <param name="securityLvl">This is the description of the service, it is Optional</param>
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
		

		/// <summary>
        /// Need method for creating ARG segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="argPos">This is position of the argument</param>
        /// <param name="argName">This is the name of the argument</param>
        /// <param name="argDataType">This is the data type of the argument</param>
        /// <param name="mandatory">This is the field specifing if the argument is mandatory or optional, the field is Optional</param>
        /// <param name="argValue">This is the value of the argument, it is Optional</param>
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
		

		/// <summary>
        /// Need method for creating RSP segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="respPos">This is position of the response</param>
        /// <param name="respName">This is the name of the response</param>
        /// <param name="respDataType">This is the data type of the response</param>
        /// <param name="respValue">This is the value of the response, it is Optional</param>
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
		

		/// <summary>
        /// Need method for creating MCH segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="IP">This is the IP of the service</param>
        /// <param name="port">This is the port to be communicated on of the service</param>
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


		/// <summary>
        /// Need method for creating PUB segment
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        /// <param name="result">This is the result of the query, either OK or NOT OK</param>
        /// <param name="errCode">This is the error code returned, it is Optional</param>
        /// <param name="errMessage">This is the error returned, it is Optional</param>
        /// <param name="numOfSegments">This is the number of segments returned, it is Optional</param>
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

		/// <summary>
        /// Adds on HL7 specific chars to make valid HL7 message 
        /// </summary>
        /// <param name="builtHL7">This is the HL7 object that is adding the segment</param>
        private static void FinalizeHL7Protocol(HL7 builtHL7)
		{

			string messagebuilder = Definitions.BEGIN_MESSAGE.ToString();

			foreach(HL7Segment seg in builtHL7.segments)
			{
				messagebuilder+= seg.segment + Definitions.END_SEGMENT.ToString();

			}

			messagebuilder += Definitions.END_MESSAGE.ToString() + Definitions.END_SEGMENT.ToString();
			builtHL7.fullHL7Message = messagebuilder;
		}
	}
}
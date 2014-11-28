using System;
using System.Collections.Generic;
using System.Net;

namespace HL7Records
{
	class TestCase
	{

		public static void Main(string[] args)
		{
			HL7Handler handler = new HL7Handler();
			//HL7 myHL7 = HL7Builder.BuildRegisterTeamMessage();
			Service myservice = new Service();
			HL7 responseHL7 = new HL7();
			CreateService(myservice);

			//get response string
			string responseString = CreateResponseString();
			
			//put response string into response handler
			responseHL7 = handler.HandleResponse(responseString);

			//string yoyo = handler.QueryServiceMessage(myservice);
			Console.WriteLine("Handle Response");
			// Console.WriteLine(myHL7.fullHL7Message);
			foreach (HL7Segment seg in responseHL7.segments)
			{
				Console.WriteLine(seg.segment);
				foreach (string field in seg.fields)
				{
					Console.WriteLine(field);
				}
			}
			
			Console.WriteLine("Build Exec Command");
			string cmd = handler.ExecuteServiceMessage(myservice);
			Console.WriteLine(cmd);
			Console.WriteLine("Build Query Service Command");
			cmd = handler.QueryServiceMessage(myservice, myservice);
			Console.WriteLine(cmd);

			return;		
		}


		public static string CreateResponseString()
		{
			char msgBeg = (char)11;
			char segEnd = (char)13;
			char msgEnd = (char)28;
			string responseString = "SOA|OK|<teamID>|<expiration>||";

			string messagebuilder = msgBeg.ToString();
			messagebuilder += responseString;

			messagebuilder += segEnd.ToString() + msgEnd.ToString() + segEnd.ToString();
			return messagebuilder;
		}
		public static void CreateService(Service myservice)
		{
			myservice.TeamName="FunnyGlasses";
			myservice.TeamID = "1186";
			myservice.Tag = "tagName";
			myservice.SecurityLevel = 3;
			myservice.Description = "description";
			myservice.IP = IPAddress.Parse("192.168.2.24");
			myservice.Port = 3128;
			
			Argument firstArg = new Argument();
			firstArg.Position = 1;
			firstArg.ArgumentName = "argName";
			firstArg.ArgumentDataType = "argDataType";
			firstArg.Mandatory = true;

			Argument secondArg = new Argument();
			secondArg.Position = 2;
			secondArg.ArgumentName = "argName2";
			secondArg.ArgumentDataType = "argDataType2";
			secondArg.Mandatory = false;
			
			myservice.Arguments.Add(firstArg);
			myservice.Arguments.Add(secondArg);

			Response firstResp = new Response();
			firstResp.Position = 1;
			firstResp.ResponseName = "respName";
			firstResp.ResponseDataType = "respDataType";

			Response secondResp = new Response();
			secondResp.Position = 2;
			secondResp.ResponseName = "respName2";
			secondResp.ResponseDataType = "respDataType2";

			Response thirdResp = new Response();
			thirdResp.Position = 3;
			thirdResp.ResponseName = "DerpyMatt";
			thirdResp.ResponseDataType = "he's a Derp";

			myservice.Responses.Add(firstResp);
			myservice.Responses.Add(secondResp);
			myservice.Responses.Add(thirdResp);
		}
	}
}
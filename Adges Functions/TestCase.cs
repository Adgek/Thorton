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
			


			string yoyo = handler.PublishServiceMessage(myservice);
			// Console.WriteLine("FINALPRINT");
			// Console.WriteLine(myHL7.fullHL7Message);
			Console.WriteLine(yoyo);
			

			return;		
		}
	}
}
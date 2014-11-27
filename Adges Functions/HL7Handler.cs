using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7Records
{
	class HL7Handler
	{
		//This is the class that handles all HL7 messages and responses
		public HL7Handler()
		{
			Console.WriteLine("Made the Handler");
		}

		//Needs a function for sending a Register Team message
		public string RegisterTeamMessage()
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Register Team message

		//Needs a function for sending a Unregister Team message
		public string UnregisterTeamMessage()
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Unregister Team message

		//Needs a function for sending a Query Team message
		public string UnregisterTeamMessage()
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Query Team message

		//Needs a function for sending a Publish Service message
		public string PublishServiceMessage()
		{
			HL7 myRecord = HL7Builder.BuildPublishServiceMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Publish Service message

		//Needs a function for sending a Query Service message
		public string QueryServiceMessage()
		{
			HL7 myRecord = HL7Builder.BuildQueryServiceMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Query Service message

		//Needs a function for sending a Execute Service message
		public string ExecuteServiceMessage()
		{
			HL7 myRecord = HL7Builder.BuildExecuteServiceMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for handling the reponse of a Execute Service message
		
	}
}
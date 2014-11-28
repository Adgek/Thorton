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
		}

		//Needs a function for sending a Register Team message
		//done
		public string RegisterTeamMessage()
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function to handle all responses
		public HL7 HandleResponse(string response)
		{
			HL7 myRecord = new HL7(response);
			return myRecord;
		}

		//Needs a function for sending a Unregister Team message
		public string UnregisterTeamMessage()
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage();

			return myRecord.fullHL7Message;
		}

		//Needs a function for sending a Query Team message
		public string QueryTeamMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildQueryTeamMessage(myService);

			return myRecord.fullHL7Message;
		}

		//Needs a function for sending a Publish Service message
		//done
		public string PublishServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildPublishServiceMessage(myService);

			return myRecord.fullHL7Message;
		}

		//Needs a function for sending a Query Service message
		public string QueryServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildQueryServiceMessage(myService);

			return myRecord.fullHL7Message;
		}

		//Needs a function for sending a Execute Service message
		public string ExecuteServiceMessage()
		{
			HL7 myRecord = HL7Builder.BuildExecuteServiceMessage();

			return myRecord.fullHL7Message;
		}
	}
}
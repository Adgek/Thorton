using HL7Lib.ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7Lib.HL7
{
	public class HL7Handler
	{
		//This is the class that handles all HL7 messages and responses
		public HL7Handler()
		{
		}

		//********************************
		//Response Handlers
		//********************************

		//Needs a function to handle all responses
		public HL7 HandleResponse(string response)
		{
			HL7 myRecord = new HL7(response);
			return myRecord;
		}

		//********************************
		//Handle Message Builders
		//********************************

		//Needs a function for sending a Register Team message
		//done
		public HL7 RegisterTeamMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage(myService);

			return myRecord;
		}

		//Needs a function for sending a Unregister Team message
		public HL7 UnregisterTeamMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildUnregisterTeamMessage(myService);

			return myRecord;
		}

		//Needs a function for sending a Query Team message
		public HL7 QueryTeamMessage(Service myService, Service queryService)
		{
			HL7 myRecord = HL7Builder.BuildQueryTeamMessage(myService, queryService);

			return myRecord;
		}

		//Needs a function for sending a Publish Service message
		//done
		public HL7 PublishServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildPublishServiceMessage(myService);

			return myRecord;
		}

		//Needs a function for sending a Query Service message
		public HL7 QueryServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildQueryServiceMessage(myService);

			return myRecord;
		}

		//Needs a function for sending a Execute Service message
		public HL7 ExecuteServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildExecuteServiceMessage(myService);

			return myRecord;
		}

        public HL7 BuildResponseMessage(Service myService)
        {
            HL7 myRecord = HL7Builder.BuildServiceResponseMessage(myService);

            return myRecord;
        }
	}
}
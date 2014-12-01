//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: HL7Handler.cs
//Date: 23/11/14
//Purpose: This file contains the handler of the HL7 protocol. It determines what a user needs within thier HL7 message to fulfill thier usecase.
//***********************

using HL7Lib.ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HL7Lib.HL7
{
	/// <summary>
    /// This is the class that handles all HL7 messages and responses
    /// Depending on the usecase it creates a different HL7 object to return.
    /// </summary>
	public class HL7Handler
	{
		/// <summary>
        /// constructor
        /// </summary>
		public HL7Handler()
		{
		}

		//********************************
		//Response Handlers
		//********************************


		/// <summary>
        /// Needs a function to handle all responses
        /// </summary>
        /// <param name="response">This is the response that needs to be handled</param>
		public HL7 HandleResponse(string response)
		{
			HL7 myRecord = new HL7(response);
			return myRecord;
		}

		//********************************
		//Handle Message Builders
		//********************************


		/// <summary>
        ///Needs a function for sending a Register Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 RegisterTeamMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildRegisterTeamMessage(myService);

			return myRecord;
		}


		/// <summary>
        /// Needs a function for sending a Unregister Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 UnregisterTeamMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildUnregisterTeamMessage(myService);

			return myRecord;
		}


		/// <summary>
        /// Needs a function for sending a Query Team message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 QueryTeamMessage(Service myService, Service queryService)
		{
			HL7 myRecord = HL7Builder.BuildQueryTeamMessage(myService, queryService);

			return myRecord;
		}


		/// <summary>
        /// Needs a function for sending a Publish Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 PublishServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildPublishServiceMessage(myService);

			return myRecord;
		}


		/// <summary>
        /// Needs a function for sending a Query Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 QueryServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildQueryServiceMessage(myService);

			return myRecord;
		}


		/// <summary>
        /// Needs a function for sending a Execute Service message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
		public HL7 ExecuteServiceMessage(Service myService)
		{
			HL7 myRecord = HL7Builder.BuildExecuteServiceMessage(myService);

			return myRecord;
		}

		/// <summary>
        /// Needs a function for building a reponse message
        /// </summary>
        /// <param name="myService">The service object that contains needed message information</param>
        /// <param name="notOkMessage">Pass true to generate a NOT OK response, it is Optional</param>
        public HL7 BuildResponseMessage(Service myService, bool notOkMessage = false)
        {
            HL7 myRecord;
            if (notOkMessage)
            {
                myRecord = HL7Builder.BuildServiceResponseNotOkMessage(myService);
            }
            else
            {
                myRecord = HL7Builder.BuildServiceResponseOkMessage(myService);
            }
            return myRecord;
        }
	}
}
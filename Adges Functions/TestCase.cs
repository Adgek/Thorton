using System;
using System.Collections.Generic;


namespace HL7Records
{
	class TestCase
	{

		public static void Main(string[] args)
		{
			//HL7 myHL7 = HL7Builder.BuildRegisterTeamMessage();
			HL7 myHL7 = HL7Builder.BuildPublishServiceMessage();
			Console.WriteLine("FINALPRINT");
			Console.WriteLine(myHL7.fullHL7Message);
			

			return;		
		}
	}
}
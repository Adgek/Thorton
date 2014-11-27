using System;
using System.Collections.Generic;


namespace test
{
	class TestCase
	{

		public static void Main(string[] args)
		{
			HL7 myHL7 = HL7Builder.BuildRegisterTeamMessage();
			
			Console.WriteLine(myHL7.segments);

			return;		
		}
	}
}
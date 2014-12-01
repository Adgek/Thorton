//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: HL7Segment.cs
//Date: 23/11/14
//Purpose: This file contains the definition of an HL7 segment
//***********************

using System;
using System.Collections.Generic;

namespace HL7Lib.HL7
{
	/// <summary>
    /// This is the HL7Segment object
    /// </summary>
    public class HL7Segment
	{

		//This List is a list of fields in the segment
		public List<string> fields;
		//This is the full segment as a string
		public string segment { get; set;}
		//This bool determines if the segment is in valid HL7 protocol
        public bool isValid { get; set; }

        /// <summary>
        /// Constructor is blank for the HL7Segment builder
        /// </summary>
		public HL7Segment()
		{
			fields = new List<string>();
            isValid = true;
		}
		
		/// <summary>
        /// Constructor takes an HL7Segment string for responses
        /// </summary>
        /// <param name="fullSegment">This is the full segment as a string</param>
        /// <param name="numFields">This is the protocols number of fields to check if segment is valid</param>
		public HL7Segment(string fullSegment, int numFields)
		{
            isValid = true;
			segment = fullSegment;

			fields = HL7Parser.GetFieldsFromSegment(fullSegment);
            
            isSegmentValid(numFields);
		}


		/// <summary>
        /// Needs a function to convert a full segment string into fields
        /// </summary>
		public void ConvertFieldsToSegmentString()
		{
			foreach (string field in fields)
			{
				segment+=field + "|";
			}
		}


		/// <summary>
        /// Need a function to check if amount of fields equals what was expected, if not then segment is not valid.
        /// </summary>
        /// <param name="numFields">This is the protocols number of fields to check if segment is valid</param>
        public void isSegmentValid(int numFields)
        {
            if (fields.Count != numFields)
            {
                isValid = false;
            }
        }


	}
}
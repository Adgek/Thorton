//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: LogUtility.cs
//Date: 23/11/14
//Purpose: This file provides a static method which iterates through message segments to log them in a nice format
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Lib;
using HL7Lib.HL7;
using NLog;

namespace ThortonSOAService
{
    /// <summary>
    /// logs segments in a readable formats
    /// </summary>
    class LogUtility
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// logs segments passed in
        /// </summary>
        /// <param name="message">hl7 object that contains the segments for logging</param>
        public static void logMessage(HL7 message)
        {
            foreach (HL7Segment seg in message.segments)
            {
                logger.Log(LogLevel.Info, "\t>> " + HL7Parser.LogSegment(seg.segment));
            }
        }
    }
}

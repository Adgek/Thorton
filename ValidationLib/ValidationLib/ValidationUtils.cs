﻿// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ValidationLib
{
    static public class ValidationUtils
    {

        static public string regex = @"^[A-Za-z0-9 _.-]+$";
        static Regex reg = new Regex(regex);
        /// <summary>
        /// Parses an int from a string
        /// Throws the exception if the string cannot be parsed
        /// </summary>
        /// <param name="number">The string to parse</param>
        /// <returns>the int that was parsed</returns>
        public static int ParseIntFromString(string number)
        {
            try
            {
                return Convert.ToInt32(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static float ParseFloatFromString(string number)
        {
            try
            {
                return (float)Convert.ToDouble(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static double ParseDoubleFromString(string number)
        {
            try
            {
                return Convert.ToDouble(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static char ParseCharFromString(string number)
        {
            try
            {
                return Convert.ToChar(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static short ParseShortFromString(string number)
        {
            try
            {
                return Convert.ToInt16(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static long ParseLongFromString(string number)
        {
            try
            {
                return Convert.ToInt64(number);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Boolean checkIfAllCharsAreValid(String test)
        {
            return reg.Match(test).Success;
        }

    }
}

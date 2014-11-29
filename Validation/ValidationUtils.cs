// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soa2.Validation
{
    static public class ValidationUtils
    {
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
    }
}

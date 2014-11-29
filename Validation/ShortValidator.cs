
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NLog;

namespace Soa2.Validation
{
    class ShortValidator : ParameterValidator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int min = 0;
        int max = 0;
        short Value;
        /// <summary>
        /// Float validator
        /// </summary>
        /// <param name="Max">the max value allowed for the float</param>
        /// <param name="Min">the min value allowed for the float</param>
        public ShortValidator(int Max, int Min)
        {
            min = Min;
            max = Max;
            RuleFor(param => param.Value).Must(BeANumber).WithMessage("This value must be a number.").Must(BeWithinRAnge).WithMessage("This value must be within the range of " + min + " to " + max + ".");
        }

        /// <summary>
        /// makes sure the value is a number
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a number, false: is not a number</returns>
        private Boolean BeAShort(string value)
        {
            try
            {
                Value = ValidationUtils.ParseShortFromString(value);
                return true;
            }
            catch(Exception e)
            {
                logger.Trace(e.Message);
                return false;
            }
        }
    }
}


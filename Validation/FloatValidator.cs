
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NLog;

namespace Soa2.Validation
{
    class FloatValidator : ParameterValidator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int min = 0;
        int max = 0;
        float Value = 0;
        /// <summary>
        /// Float validator
        /// </summary>
        /// <param name="Max">the max value allowed for the float</param>
        /// <param name="Min">the min value allowed for the float</param>
        public FloatValidator(int Max, int Min)
        {
            min = Min;
            max = Max;
            RuleFor(param => param.Value).Must(BeANumber).WithMessage("This value must be a number.").Must(BeWithinRAnge).WithMessage("This value must be within the range of " + min + " to " + max + ".");
        }

        /// <summary>
        /// Checks the range of the value passed in
        /// </summary>
        /// <param name="value">this value is not actually used but is required by the fluent validation method.</param>
        /// <returns>true: is within range, false: is not within range</returns>
        private Boolean BeWithinRAnge(string value)
        {
            if(Value >= min && Value <= max)            
                return true;
            return false;
        }

        /// <summary>
        /// makes sure the value is a number
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a number, false: is not a number</returns>
        private Boolean BeANumber(string value)
        {
            try
            {
                Value = ValidationUtils.ParseFloatFromString(value);
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


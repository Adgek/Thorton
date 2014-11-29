
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NLog;

namespace Soa2.Validation
{
    class DoubleValidator : ParameterValidator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        double Value;
        /// <summary>
        /// Double validator
        /// </summary>
        public DoubleValidator()
        {
            RuleFor(param => param.Value).Must(BeADouble).WithMessage("This value must be a Double.").Must(BeWithinRAnge).WithMessage("This value must be within the range of " + min + " to " + max + ".");
        }

        /// <summary>
        /// makes sure the value is a Double
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a Double, false: is not a Double</returns>
        private Boolean BeADouble(string value)
        {
            try
            {
                Value = ValidationUtils.ParseDoubleFromString(value);
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


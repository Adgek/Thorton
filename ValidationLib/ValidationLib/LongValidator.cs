
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace ValidationLib
{
    class LongValidator : MessageValidator
    {
        

        long Value;
        /// <summary>
        /// Long validator
        /// </summary>
        public LongValidator()
        {
            RuleFor(param => param.Value).Must(BeALong).WithMessage("This value must be a Long.");
        }

        /// <summary>
        /// makes sure the value is a Long
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a Long, false: is not a Long</returns>
        private Boolean BeALong(string value)
        {
            try
            {
                Value = ValidationUtils.ParseLongFromString(value);
                return true;
            }
            catch(Exception e)
            {
                
                return false;
            }
        }
    }
}


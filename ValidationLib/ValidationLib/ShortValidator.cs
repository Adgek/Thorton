
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace ValidationLib
{
    class ShortValidator : MessageValidator
    {
        

        short Value;
        /// <summary>
        /// Short validator
        /// </summary>
        public ShortValidator()
        {
            RuleFor(param => param.Value).Must(BeAShort).WithMessage("This value must be a Short.");
        }

        /// <summary>
        /// makes sure the value is a short
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a short, false: is not a short</returns>
        private Boolean BeAShort(string value)
        {
            try
            {
                Value = ValidationUtils.ParseShortFromString(value);
                return true;
            }
            catch(Exception e)
            {
                
                return false;
            }
        }
    }
}


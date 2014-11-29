
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace ValidationLib
{
    class CharValidator : MessageValidator
    {

        char Value;

        /// <summary>
        /// Char validator
        /// </summary>
        public CharValidator()
        {
			RuleFor(param => param.Value).Must(BeAChar).WithMessage("This value must be a Char.");
        }

        /// <summary>
        /// makes sure the value is a char
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a char, false: is not a char</returns>
        private Boolean BeAChar(string value)
        {
            try
            {
                Value = ValidationUtils.ParseCharFromString(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


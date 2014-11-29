// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ValidationLib
{
    class BooleanValidator : MessageValidator
    {
        /// <summary>
        /// Makes sure the value is a valid boolean string
        /// </summary>
        public BooleanValidator()
        {
            RuleFor(param => param.Value).Must(BeAValidBooleanString).WithMessage("You must enter a valid boolean value(true, false, 1, 0)");
        }

        /// <summary>
        /// Checks if the string passed in is a valid boolean representation
        /// </summary>
        /// <param name="value">the value to check</param>
        /// <returns>true: is a boolean, false: is not a boolean</returns>
        private bool BeAValidBooleanString(string value)
        {
            if (value == "true" || value == "false" ||  value == "0" || value == "1")
                return true;
            return false;
        }
    }
}

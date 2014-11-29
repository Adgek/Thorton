// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ValidationLib
{
    class IntValidator : MessageValidator
    {

        int Value = 0;
        /// <summary>
        /// Int validator
        /// </summary>
        public IntValidator(int Max, int Min)
        {
            RuleFor(param => param.Value).Must(BeANumber).WithMessage("This value must be an Integer.");
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
                Value = ValidationUtils.ParseIntFromString(value);
                return true;
            }
            catch(Exception e)
            {
                
                return false;
            }
        }
    }
}

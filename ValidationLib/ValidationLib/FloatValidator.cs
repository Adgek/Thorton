﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ValidationLib
{
    class FloatValidator : MessageValidator
    {

        float Value = 0;
        /// <summary>
        /// Float validator
        /// </summary>
        public FloatValidator()
        {
            RuleFor(param => param.Value).Must(BeAFloat).WithMessage("This value must be a Float.");
        }

        /// <summary>
        /// makes sure the value is a Float
        /// </summary>
        /// <param name="value">the string to check</param>
        /// <returns>true: is a Float, false: is not a Float</returns>
        private Boolean BeAFloat(string value)
        {
            try
            {
                Value = ValidationUtils.ParseFloatFromString(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


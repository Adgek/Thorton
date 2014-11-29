// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ValidationLib
{
    class StringValidator : MessageValidator
    {
            /// <summary>
            /// Short validator
            /// </summary>
            public StringValidator()
            {
                RuleFor(param => param.Value).Must(MatchValidCharacters).WithMessage("This value must only contain the following characters : A-Z a-z 0-9 _.-");
            }

            /// <summary>
            /// makes sure the value is a short
            /// </summary>
            /// <param name="value">the string to check</param>
            /// <returns>true: is a short, false: is not a short</returns>
            private Boolean MatchValidCharacters(string value)
            {
                return ValidationUtils.checkIfAllCharsAreValid(value);                
            }
        
    }
}

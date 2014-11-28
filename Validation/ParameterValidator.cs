// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Soa2.NestedConfig;

namespace Soa2.Validation
{
    public class ParameterValidator : AbstractValidator<Parameter>
    {
        /// <summary>
        /// Base class for parameter validators
        /// Makes sure there has at least been a value entered
        /// </summary>
        public ParameterValidator()
        {
            RuleFor(param => param.Value).NotEmpty().WithMessage("You must enter a value");
        }
    }
}

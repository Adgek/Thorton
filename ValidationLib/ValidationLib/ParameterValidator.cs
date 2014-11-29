// By: Kyle Fowler and Matthew Anselmo
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HL7Lib.ServiceData;

namespace ValidationLib
{
    public class MessageValidator : AbstractValidator<Message>
    {
        /// <summary>
        /// Base class for parameter validators
        /// Makes sure there has at least been a value entered
        /// </summary>
        public MessageValidator()
        {
            RuleFor(param => param.Value).NotEmpty().WithMessage("You must enter a value");
        }
    }
}

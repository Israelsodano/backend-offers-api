using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Offers.Domain.Entities;

namespace Offers.Application.Validators
{
    public class CampusValidator : AbstractValidator<Campus>
    {
        public CampusValidator()
        {
            RuleFor(x => x.City)
                .NotNull()
                .NotEmpty();
            
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}

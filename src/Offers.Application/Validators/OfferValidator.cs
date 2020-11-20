using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Offers.Domain.Entities;

namespace Offers.Application.Validators
{
    public class OfferValidator : AbstractValidator<Offer>
    {
        public OfferValidator()
        {
            RuleFor(x => x.FullPrice)
                .Must(x => x > 0);

            RuleFor(x => x.StartDate)
                .NotEmpty();
        }
    }
}

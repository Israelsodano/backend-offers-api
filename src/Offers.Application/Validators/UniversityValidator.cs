using FluentValidation;
using Offers.Domain.Entities;

namespace Offers.Application.Validators
{
    public class UniversityValidator : AbstractValidator<University>
    {
        public UniversityValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Score)
                .Must(x => x > 0);

            RuleFor(x => x.LogoUrl)
               .NotNull()
               .NotEmpty();
        }
    }
}

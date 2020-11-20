using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Offers.Domain.Entities;

namespace Offers.Application.Validators
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Kind)
                .IsInEnum();

            RuleFor(x => x.Level)
                .IsInEnum();

            RuleFor(x => x.Shift)
                .IsInEnum();
        }
    }
}

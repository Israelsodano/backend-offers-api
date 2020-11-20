using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Common.Utils
{
    public static class FluentValidatorUtils
    {
        public static string GetErrorMessage(this IEnumerable<ValidationFailure> failures) => 
            string.Join(", ", failures.Select(x=> $"{x.PropertyName}: '{x.ErrorCode} - {x.ErrorMessage}'"));
    }
}

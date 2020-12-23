using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CostsDiary.Api.UnitTests.Extensions
{
    public static class ValidatorFailureExtension
    {
        public static void ContainsErrorMessage(this IEnumerable<ValidationFailure> errors, string propertyName, string errorMessage)
        {
            var msg = errors.Single(e => e.PropertyName == propertyName).ErrorMessage;
            
            msg.Contains(errorMessage).Should().BeTrue(because: $"Error Message: '{msg}' does not contain '{errorMessage}'.");
        }

        public static void WhenErrorMessageContains(this IEnumerable<ValidationFailure> errors, string errorMessage)
        {
            errors.When(x => x.ErrorMessage.Contains(errorMessage));
        }
    }
}

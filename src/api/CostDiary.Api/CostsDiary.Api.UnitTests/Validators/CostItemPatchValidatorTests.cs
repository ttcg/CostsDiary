using Bogus;
using CostsDiary.Api.UnitTests.Extensions;
using CostsDiary.Api.Web.Validators;
using CostsDiary.Api.Web.ViewModels;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CostsDiary.Api.UnitTests.Validators
{
    public class CostItemPatchValidatorTests
    {
        private CostItemPatchValidator validator;
        private Faker<CostItemPatchViewModel> faker;

        public CostItemPatchValidatorTests()
        {
            validator = new CostItemPatchValidator();

            faker = new Faker<CostItemPatchViewModel>()
                .RuleFor(x => x.CostTypeId, f => f.Random.Guid())
                .RuleFor(x => x.DateUsed, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(x => x.ItemName, f => f.Commerce.ProductName())
                .RuleFor(x => x.Amount, f => f.Finance.Amount(min: 1, max: 50));
        }

        [Fact]
        public void Should_have_error_when_properties_are_empty()
        {
            var model = new CostItemPatchViewModel();
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ItemName);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
            result.ShouldHaveValidationErrorFor(x => x.CostTypeId);
            result.ShouldHaveValidationErrorFor(x => x.DateUsed);
        }

        [Fact]
        public void Should_not_have_error_when_properties_are_valid()
        {
            var model = faker.Generate();
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CostTypeId);
            result.ShouldNotHaveValidationErrorFor(x => x.DateUsed);
            result.ShouldNotHaveValidationErrorFor(x => x.ItemName);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [Fact]
        public void Should_have_error_when_DateUsed_is_InTheFuture()
        {
            var model = new CostItemPatchViewModel
            {
                DateUsed = DateTime.Now.AddDays(1)
            };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DateUsed).WhenErrorMessageContains("must be less than");
        }

        [Fact]
        public void Should_have_error_when_Amount_is_Invalid()
        {
            var model = new CostItemPatchViewModel
            {
                Amount = 0.5m
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Amount).WhenErrorMessageContains("must be between 1 and 10000");
        }
    }
}

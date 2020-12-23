using Bogus;
using CostsDiary.Api.UnitTests.Extensions;
using CostsDiary.Api.Web.Validators;
using CostsDiary.Api.Web.ViewModels;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace CostsDiary.Api.UnitTests.Validators
{
    public class CostItemCreateValidatorTests
    {
        private CostItemCreateValidator validator;
        private Faker<CostItemCreateViewModel> faker;

        public CostItemCreateValidatorTests()
        {
            validator = new CostItemCreateValidator();

            faker = new Faker<CostItemCreateViewModel>()
                .RuleFor(x => x.CostItemId, f => f.Random.Guid())
                .RuleFor(x => x.CostTypeId, f => f.Random.Guid()) 
                .RuleFor(x => x.DateUsed, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(x => x.ItemName, f => f.Commerce.ProductName())
                .RuleFor(x => x.Amount, f => f.Finance.Amount(min:1, max: 50));
        }

        [Fact]
        public void Should_have_error_when_properties_are_empty()
        {
            var model = new CostItemCreateViewModel();
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
            result.ShouldNotHaveValidationErrorFor(x => x.CostItemId);
            result.ShouldNotHaveValidationErrorFor(x => x.CostTypeId);
            result.ShouldNotHaveValidationErrorFor(x => x.DateUsed);
            result.ShouldNotHaveValidationErrorFor(x => x.ItemName);
            result.ShouldNotHaveValidationErrorFor(x => x.Amount);
        }

        [Fact]
        public void Should_have_error_when_DateUsed_is_InTheFuture()
        {
            var model = new CostItemCreateViewModel
            {
                DateUsed = DateTime.Now.AddDays(1)
            };
            var result = validator.TestValidate(model);
            
            result.ShouldHaveValidationErrorFor(x => x.DateUsed).ContainsErrorMessage(nameof(CostItemCreateViewModel.DateUsed), "must be less than");
        }

        [Fact]
        public void Should_have_error_when_Amount_is_Invalid()
        {
            var model = new CostItemCreateViewModel
            {
                Amount = 0.5m
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Amount).WhenErrorMessageContains("must be between 1 and 10000");
        }

        [Fact]
        public void Should_have_error_when_ItemId_is_InValid()
        {
            var model = new CostItemCreateViewModel
            {
                CostItemId = Guid.Empty
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CostItemId).WhenErrorMessageContains($"must not be equal to '{Guid.Empty}'");
        }
    }
}

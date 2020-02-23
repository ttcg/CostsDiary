using CostsDiary.Api.Web.ViewModels;
using FluentValidation;
using System;

namespace CostsDiary.Api.Web.Validators
{
    public class CostItemCreateValidator : AbstractValidator<CostItemCreateViewModel>
	{
		public CostItemCreateValidator()
		{
			RuleFor(x => x.CostItemId).NotEqual(Guid.Empty).When(x=> x.CostItemId.HasValue);
			RuleFor(x => x.CostTypeId).NotEmpty();
			RuleFor(x => x.ItemName).Length(3, 50);
			RuleFor(x => x.DateUsed).NotEmpty().LessThan(DateTime.Now.Date.AddDays(1));
			RuleFor(x => x.Amount).InclusiveBetween(1, 10000);
		}    
    }
}

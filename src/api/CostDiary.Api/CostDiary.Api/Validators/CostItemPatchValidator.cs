using CostsDiary.Api.Web.ViewModels;
using FluentValidation;
using System;

namespace CostsDiary.Api.Web.Validators
{
    public class CostItemPatchValidator : AbstractValidator<CostItemPatchViewModel>
	{
		public CostItemPatchValidator()
		{
			RuleFor(x => x.CostTypeId).NotEmpty();
			RuleFor(x => x.ItemName).Length(3, 50);
			RuleFor(x => x.DateUsed).NotEmpty().LessThan(DateTime.Now.Date.AddDays(1));
			RuleFor(x => x.Amount).InclusiveBetween(1, 10000);
		}    
    }
}

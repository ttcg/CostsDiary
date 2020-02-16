using System;

namespace CostsDiary.Api.Web.ViewModels
{
    public class CostItemViewModel
    {
        public Guid CostItemId { get; set; }
        public string ItemName { get; set; }
        public CostTypeViewModel CostType { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateUsed { get; set; }
    }
}

using System;

namespace CostsDiary.Api.Data.Entities
{
    public class CostItem
    {
        public Guid CostItemId { get; set; }
        public string ItemName { get; set; }
        public Guid CostTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateUsed { get; set; }
    }
}

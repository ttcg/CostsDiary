using System;
using System.ComponentModel.DataAnnotations;

namespace CostsDiary.Api.Web.ViewModels
{
    public class CostItemPatchViewModel
    {        
        [Required]
        public string ItemName { get; set; }
        [Required]
        public Guid CostTypeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime DateUsed { get; set; }
    }
}

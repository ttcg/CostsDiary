using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostsDiary.Web.Models
{
    public class CostTypeModel
    {
        [Required]
        public int CostTypeId { get; set; }

        [Required]
        public string CostTypeDescription { get; set; }
    }
}

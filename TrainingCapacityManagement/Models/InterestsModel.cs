using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TrainingCapacityManagement.Areas.Identity.Data;

namespace TrainingCapacityManagement.Models
{
    public class Interest
    {
        public Guid Id { get; set; }
        public Sport Sport { get; set; }
        public ApplicationUser User { get; set; }
    }

    //Used in InterestSelector
    [NotMapped]
    public class SportsSelector
    {
        public Sport Sport { get; set; }
        public bool IsChecked { get; set; }
    }
}

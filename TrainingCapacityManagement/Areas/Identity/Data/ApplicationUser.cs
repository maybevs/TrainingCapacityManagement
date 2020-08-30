using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrainingCapacityManagement.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }
        [PersonalData]
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
    }
}

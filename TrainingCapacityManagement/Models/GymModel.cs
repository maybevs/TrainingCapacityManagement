using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingCapacityManagement.Models
{
    public class Gym
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Ort")]
        public string Name { get; set; }
        [Display(Name = "Straße")]
        public string StreetAddress { get; set; }
        [Display(Name = "Ort")]
        public string City { get; set; }

    }

    public class GymSegment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Hallenabschnitt")]
        public string Name { get; set; }
        [Display(Name = "Turnhalle")]
        public Gym Gym { get; set; }
        public string GymSelection { get; set; }
    }
}

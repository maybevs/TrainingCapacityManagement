using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingCapacityManagement.Models
{
    public class Training
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Sportart")]
        public Sport Sport { get; set; }
        [Display(Name = "Trainingsbeginn")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Trainingsende")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Halle")]
        public Gym Gym { get; set; }
        [Display(Name = "Teilnahmerkapazität")]
        public int Capacity { get; set; }
        [Display(Name = "Haushalte können sich Plätze teilen")]
        public bool HouseholdsSharePlace { get; set; }
        [Display(Name = "Anmeldenachricht", Description = "Diese Nachricht wird dem Teilnehmer bei der Anmeldung angezeigt.")]
        public string SignupMessage { get; set; }
        [Display(Name = "Hygienekonzept", Description = "Link zum Hygienekonzept")]
        public string SafetyConceptURL { get; set; }

        public int GymSelection { get; set; }
        public int SportsSelection { get; set; }
    }

    public class Sport
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Sportart")]
        public string Name { get; set; }

    }

    public class TrainingsRegistration
    {
        [Key]
        public int Id { get; set; }

        public Training Training { get; set; }
        public string UserId { get; set; }

        public int tid { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingCapacityManagement.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Überschrift")]
        public string Title { get; set; }
        [Display(Name = "Text")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}

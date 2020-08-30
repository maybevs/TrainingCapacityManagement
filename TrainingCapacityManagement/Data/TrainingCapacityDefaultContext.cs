using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingCapacityManagement.Models;
using TrainingCapacityManagement.Areas.Identity.Data;

namespace TrainingCapacityManagement.Data
{
    public class TrainingCapacityDefaultContext : IdentityDbContext<ApplicationUser>
    {
        public TrainingCapacityDefaultContext(DbContextOptions<TrainingCapacityDefaultContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingCapacityManagement.Models.Gym> Gym { get; set; }

        public DbSet<TrainingCapacityManagement.Models.GymSegment> GymSegment { get; set; }

        public DbSet<TrainingCapacityManagement.Models.Sport> Sport { get; set; }

        public DbSet<TrainingCapacityManagement.Models.Training> Training { get; set; }

        public DbSet<TrainingCapacityManagement.Models.TrainingsRegistration> TrainingsRegistration { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

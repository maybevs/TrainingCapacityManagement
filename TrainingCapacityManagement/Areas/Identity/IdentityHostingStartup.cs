using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainingCapacityManagement.Areas.Identity.Data;
using TrainingCapacityManagement.Data;

[assembly: HostingStartup(typeof(TrainingCapacityManagement.Areas.Identity.IdentityHostingStartup))]
namespace TrainingCapacityManagement.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<TrainingCapacityDefaultContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TrainingCapacityDefaultContext")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<TrainingCapacityDefaultContext>();
            });
        }
    }
}
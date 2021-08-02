using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BkpGasProcurementSystem.Data;
using Microsoft.AspNetCore.Identity;
using BkpGasProcurementSystem.Areas.Identity.Data;

namespace BkpGasProcurementSystem.Models
{
    public class SeedAdmin
    {

        public async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BkpGasProcurementSystemIdentityContext(
                                        serviceProvider.GetRequiredService<DbContextOptions<BkpGasProcurementSystemIdentityContext>>()))
            {
                // Look for any flower.
                if (context.Users.Any())
                {
                    return;
                    // DB has been seeded
                }
                var admin = new BkpGasProcurementSystemUser
                {
                    FullName = "admin",
                    Email = "admin@bkp.com",
                    Address = "BKP",
                    PhoneNumber = "9998887771"
                };
                //var userManager = UserManager< BkpGasProcurementSystemUser >
                //var result = await UserManager.CreateAsync(admin, "Bkp@1234");
            }
        }
    }
}


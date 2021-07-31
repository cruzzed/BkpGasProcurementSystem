using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Areas.Identity.Data
{
    public enum Roles
    {
        Admin,
        Customer,
        Delivery,
    }
    public static class ContextRoles
    {
        public static async Task SeedRolesAsync(UserManager<BkpGasProcurementSystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Delivery.ToString()));
        }
    }
}

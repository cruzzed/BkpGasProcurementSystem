using System;
using BkpGasProcurementSystem.Areas.Identity.Data;
using BkpGasProcurementSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BkpGasProcurementSystem.Areas.Identity.IdentityHostingStartup))]
namespace BkpGasProcurementSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BkpGasProcurementSystemIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BkpGasProcurementSystemIdentityContextConnection")));

                services.AddDefaultIdentity<BkpGasProcurementSystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BkpGasProcurementSystemIdentityContext>();
            });
        }
    }
}
using BkpGasProcurementSystem.Areas.Identity.Data;
using BkpGasProcurementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<BkpGasProcurementSystemUser> userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<BkpGasProcurementSystemUser> usrMgr)
        {
            _logger = logger;
            userManager = usrMgr;

        }

        public IActionResult registerUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> registerUser(registerUser user)
        {
            if (ModelState.IsValid)
            {
                BkpGasProcurementSystemUser webUser = new BkpGasProcurementSystemUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FullName = user.FullName,
                    EmailConfirmed = true,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };

                IdentityResult result = await userManager.CreateAsync(webUser, user.Password);
                if (result.Succeeded)
                {
                    if (user.UserRole.Equals("Customer"))
                    {
                        await userManager.AddToRoleAsync(webUser, Roles.Customer.ToString());
                    }
                    else if (user.UserRole.Equals("Admin"))
                    {
                        await userManager.AddToRoleAsync(webUser, Roles.Admin.ToString());
                    }
                    else if (user.UserRole.Equals("Delivery"))
                    {
                        await userManager.AddToRoleAsync(webUser, Roles.Delivery.ToString());
                    }
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToAction("registerUser", "Home");
                }

            }
            return View(user);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BkpGasProcurementSystem.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BkpGasProcurementSystemUser class
    public class BkpGasProcurementSystemUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string Address { get; set; }
  
    }
}

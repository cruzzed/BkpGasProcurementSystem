using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Models;

namespace BkpGasProcurementSystem.Data
{
    public class BkpGasProcurementSystemOrdersContext : DbContext
    {
        public BkpGasProcurementSystemOrdersContext (DbContextOptions<BkpGasProcurementSystemOrdersContext> options)
            : base(options)
        {
        }

        public DbSet<BkpGasProcurementSystem.Models.Orders> Orders { get; set; }
    }
}

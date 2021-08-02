using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Models;

namespace BkpGasProcurementSystem.Data
{
    public class BkpGasProcurementSystemContext : DbContext
    {
        public BkpGasProcurementSystemContext (DbContextOptions<BkpGasProcurementSystemContext> options)
            : base(options)
        {
        }

        public DbSet<BkpGasProcurementSystem.Models.Deliveries> Deliveries { get; set; }

        public DbSet<BkpGasProcurementSystem.Models.Orders> Orders { get; set; }

        public DbSet<BkpGasProcurementSystem.Models.Product> Product { get; set; }

    }
}

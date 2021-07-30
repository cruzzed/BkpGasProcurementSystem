using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Models;

namespace BkpGasProcurementSystem.Data
{
    public class BkpGasProcurementSystemDeliveriesContext : DbContext
    {
        public BkpGasProcurementSystemDeliveriesContext (DbContextOptions<BkpGasProcurementSystemDeliveriesContext> options)
            : base(options)
        {
        }

        public DbSet<BkpGasProcurementSystem.Models.Deliveries> Deliveries { get; set; }
    }
}

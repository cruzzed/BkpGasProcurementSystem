using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BkpGasProcurementSystem.Models;

namespace BkpGasProcurementSystem.Data
{
    public class BkpGasProcurementSystemProductContext : DbContext
    {
        public BkpGasProcurementSystemProductContext (DbContextOptions<BkpGasProcurementSystemProductContext> options)
            : base(options)
        {
        }

        public DbSet<BkpGasProcurementSystem.Models.Product> Product { get; set; }
    }
}

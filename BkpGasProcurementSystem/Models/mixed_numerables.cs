using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Models
{
    public class mixed_numerables
    {
         public IEnumerable<Orders> ordernumerable { get; set; }
        public IEnumerable<Product> productnumerable { get; set; }
        public IEnumerable<Deliveries> deliverynumerable { get; set; }
    }
}

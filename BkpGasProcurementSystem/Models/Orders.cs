using BkpGasProcurementSystem.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Models
{


    public class Orders
    {
        public int ID { get; set; }
        public DateTime order_date { get; set; }
        public List<Product> products { get; set; }
        public String username { get; set; }
        public String address { get; set; }
        public String phone { get; set; }
        public float total_price { get; set; }
        public String Payment_status { get; set; }

    }
}
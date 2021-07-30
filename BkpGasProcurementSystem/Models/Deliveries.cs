using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Models
{
    public class Deliveries
    {
        public int ID { get; set; }

        //public Order orders { get; set; }
        public List<update_delivery> delivery_history { get; set; }
        public String status { get; set; }
        [DataType(DataType.Date)]
        public DateTime ship_time { get; set; }
    }
}

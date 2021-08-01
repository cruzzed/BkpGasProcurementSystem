using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Models
{
    public class Deliveries
    {
        [Key]
        public int ID { get; set; }

        public Orders orders { get; set; }
        public List<update_delivery> delivery_history { get; set; }
        public String status { get; set; }
        
        public DateTime ship_time { get; set; }
        public String username { get; set; }
        public Deliveries()
        {
           delivery_history = new List<update_delivery>();
        }
    }
}

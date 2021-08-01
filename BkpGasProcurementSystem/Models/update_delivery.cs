using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BkpGasProcurementSystem.Models
{
    public class update_delivery
    {
        public int ID { get; set; }

        [DataType(DataType.MultilineText)]
        public string message { get; set; }
        
        public DateTime update_when { get; set; }

        public string status { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace BkpGasProcurementSystem.Models
{
    public class Product
    {

        public int Id { get; set; }

        public byte[] Picture { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }

        public int Weight { get; set; }

    }
}
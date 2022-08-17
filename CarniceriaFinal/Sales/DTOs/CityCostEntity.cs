using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class CityCostEntity
    {
        public string? label { get; set; }
        public int value { get; set; }
        public double title { get; set; }
    }
    public class ProvincesCityCost
    {
        public string? label { get; set; }
        public string? value { get; set; }
        
        public List<CityCostEntity> items { get; set; }
    }
}

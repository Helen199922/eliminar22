using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Models
{
    public class SaleDetailEntity
    {
        public int? idVenta {get; set;}
        public int cantidad {get; set;}
        public float precio {get; set;}
        public int? idPromocion {get; set;}
        public int? idMembresiaInUser { get; set; }
        public int idProducto {get; set;}
        public float descuentoTotal {get; set;}
    }
}

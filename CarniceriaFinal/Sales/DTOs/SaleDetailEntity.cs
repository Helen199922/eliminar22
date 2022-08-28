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

    public class SaleDetailCotizacionEntity
    {
        public float idVenta { get; set; }
        public float descuentoTotalVenta { get; set; }
        public float subtotalVenta { get; set; }
        public float total { get; set; }
        public List<ProductsDetailCotizacionEntity> products { get; set; }
    }

    public class ProductsDetailCotizacionEntity
    {
        public string titulo { get; set; }
        public int cantidad { get; set; }
        public float descuentoTotalProducto { get; set; }
        public string motivoDesc { get; set; }
        public float precioFinalProducto { get; set; }
    }
}

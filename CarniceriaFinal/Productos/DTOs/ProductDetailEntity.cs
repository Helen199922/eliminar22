using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductDetailEntity
    {
        public int? IdDetalleProducto { get; set; }
        public string TituloDetalle { get; set; }
        public string Descripcion { get; set; }
        public string UrlImg { get; set; }
        public int IdProducto { get; set; }
    }
}

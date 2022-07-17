using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductEntity
    {
        public int? IdProducto { get; set; }
        public string ImgUrl { get; set; }
        public string Descripcion { get; set; }
        public float? Precio { get; set; }
        public string Titulo { get; set; }
        public int? Status { get; set; }
        public int? IdPromocion { get; set; }
        public int IdUnidad { get; set; }
        public int Stock { get; set; }
        public string minimaUnidad { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class CategoryEntity
    {
        public int? idCategoria { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int? status { get; set; }
        public string UrlImage { get; set; }
    }
}

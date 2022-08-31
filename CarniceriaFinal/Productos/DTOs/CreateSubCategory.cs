using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{

    public class SimpleProductInSubCategory
    {
        public int idProducto { get; set; }
        public string? titulo { get; set; }
        public Boolean? isInCategory { get; set; }
        public string? imgUrl { get; set; }

    }
    public class CreateSubCategory : SubCategoriaProductoEntity
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public List<CategoryEntity> Categories { get; set; }
        
    }
}

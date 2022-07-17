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
        public Boolean isActivated { get; set; }

    }
    public class CreateSubCategory : SubCategoriaProductoEntity
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public List<CategoryEntity> Categories { get; set; }
        public List<SimpleProductInSubCategory> Products { get; set; }
    }
}

using CarniceriaFinal.Productos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Models
{
    public class CategoriaProductoEntity : CategoryEntity
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public CategoriaProductoEntity()
        {
            Products = new();
        }
        public List<SimpleProductInSubCategory> Products { get; set; }
    }
}

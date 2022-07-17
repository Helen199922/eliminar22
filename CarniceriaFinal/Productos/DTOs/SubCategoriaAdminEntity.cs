using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class SubCategoriaAdminEntity : SubCategoriaProductoEntity
    {
        public int numCategories { get; set; }
    }
}

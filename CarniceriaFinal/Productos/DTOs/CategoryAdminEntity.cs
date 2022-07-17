using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class CategoryAdminEntity : CategoryEntity
    {
        public int numSubCategories { get; set; }
    }
}

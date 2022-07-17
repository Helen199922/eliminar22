using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductTableAdminEntity : ProductEntity
    {
        public string NamePromotion { get; set; }
        public List<string> NameCategories { get; set; }
    }
}

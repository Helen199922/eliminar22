using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductAdminCompleteEntity
    {
        public ProductEntity product { get; set; }
        public MeasureUnitEntity unidadMedida { get; set; }
        public List<ProductDetailEntity> detail { get; set; }
        public List<CategoriaProductoEntity> categories { get; set; }
    }
}

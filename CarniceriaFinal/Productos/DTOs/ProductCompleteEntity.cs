using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductCompleteEntity
    {
        public ProductEntity product { get; set; }
        public PromotionEntity promotion{ get; set; }
        public MeasureUnitEntity unidadMedida { get; set; }
        public List<ProductDetailEntity> detail { get; set; }
    }
}

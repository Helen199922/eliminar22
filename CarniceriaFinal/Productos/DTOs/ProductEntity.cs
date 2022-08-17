using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class PromotionsInProduct
    {
        public string title { get; set; }
        public DateTime finish { get; set; }
        public Boolean hasPromotion { get; set; }
        public List<ProductEntity> products { get; set; }
    }
    public class ProductPromotionEntity
    {
        public string? promotionValue { get; set; }
        public float? newValue { get; set; }
        public float? oldValue { get; set; }
        public int idPromotion { get; set; }
    }
    public class ProductEntity
    {
        public int? IdProducto { get; set; }
        public string? ImgUrl { get; set; }
        public string? Descripcion { get; set; }
        public float? Precio { get; set; }
        public string? Titulo { get; set; }
        public int? Status { get; set; }
        public int IdUnidad { get; set; }
        public int Stock { get; set; }
        public string? minimaUnidad { get; set; }
        public ProductPromotionEntity? ProductPromotionEntity { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class ProductSaveRequestEntity : ProductEntity
    {
        public int? CategoriyId { get; set; }
        public List<ProductDetailEntity> detail { get; set; }
    }
}

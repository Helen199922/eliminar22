using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Keyless]
    [Table("promocion_in_producto")]
    [Index("IdProducto", Name = "idProducto")]
    [Index("IdPromocion", Name = "idPromocion")]
    public partial class PromocionInProducto
    {
        [Column("idPromocion")]
        public int IdPromocion { get; set; }
        [Column("idProducto")]
        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto IdProductoNavigation { get; set; } = null!;
        [ForeignKey("IdPromocion")]
        public virtual Promocion IdPromocionNavigation { get; set; } = null!;
    }
}

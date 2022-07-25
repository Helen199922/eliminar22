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
    [Index("IdPromocion", Name = "promocion_in_producto_ibfk_2")]
    public partial class PromocionInProducto
    {
        [Column("idProducto")]
        public int? IdProducto { get; set; }
        [Column("idPromocion")]
        public int? IdPromocion { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Producto? IdProductoNavigation { get; set; }
        [ForeignKey("IdPromocion")]
        public virtual Promocion? IdPromocionNavigation { get; set; }
    }
}

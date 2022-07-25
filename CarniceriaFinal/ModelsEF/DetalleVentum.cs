using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("detalle_venta")]
    [Index("IdProducto", Name = "idProducto")]
    [Index("IdPromocion", Name = "promocion_promocion")]
    [Index("IdVenta", Name = "venta_venta")]
    public partial class DetalleVentum
    {
        [Key]
        [Column("idDetalleVenta")]
        public int IdDetalleVenta { get; set; }
        [Column("idVenta")]
        public int? IdVenta { get; set; }
        [Column("cantidad")]
        public int? Cantidad { get; set; }
        [Column("precio")]
        public double? Precio { get; set; }
        [Column("idPromocion")]
        public int? IdPromocion { get; set; }
        [Column("idProducto")]
        public int? IdProducto { get; set; }
        [Column("descuento")]
        public float? Descuento { get; set; }

        [ForeignKey("IdProducto")]
        [InverseProperty("DetalleVenta")]
        public virtual Producto? IdProductoNavigation { get; set; }
        [ForeignKey("IdPromocion")]
        [InverseProperty("DetalleVenta")]
        public virtual Promocion? IdPromocionNavigation { get; set; }
        [ForeignKey("IdVenta")]
        [InverseProperty("DetalleVenta")]
        public virtual Ventum? IdVentaNavigation { get; set; }
    }
}

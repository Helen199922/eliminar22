using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("preparacion_producto_in_producto")]
    [Index("IdPreparacionProducto", Name = "idPreparacionProducto")]
    [Index("IdProducto", Name = "idProducto")]
    public partial class PreparacionProductoInProducto
    {
        [Column("idPreparacionProducto")]
        public int? IdPreparacionProducto { get; set; }
        [Column("idProducto")]
        public int? IdProducto { get; set; }
        [Key]
        [Column("idPreparacionInProducto")]
        public int IdPreparacionInProducto { get; set; }

        [ForeignKey("IdPreparacionProducto")]
        [InverseProperty("PreparacionProductoInProductos")]
        public virtual PreparacionProducto? IdPreparacionProductoNavigation { get; set; }
        [ForeignKey("IdProducto")]
        [InverseProperty("PreparacionProductoInProductos")]
        public virtual Producto? IdProductoNavigation { get; set; }
    }
}

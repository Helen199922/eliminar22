using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("momento_degustacion_in_producto")]
    [Index("IdMomentoDegustacion", Name = "idMomentoDegustacion")]
    [Index("IdProducto", Name = "idProducto")]
    public partial class MomentoDegustacionInProducto
    {
        [Key]
        [Column("idMomentoDegustacionInProducto")]
        public int IdMomentoDegustacionInProducto { get; set; }
        [Column("idMomentoDegustacion")]
        public int IdMomentoDegustacion { get; set; }
        [Column("idProducto")]
        public int IdProducto { get; set; }

        [ForeignKey("IdMomentoDegustacion")]
        [InverseProperty("MomentoDegustacionInProductos")]
        public virtual MomentoDegustacion IdMomentoDegustacionNavigation { get; set; } = null!;
        [ForeignKey("IdProducto")]
        [InverseProperty("MomentoDegustacionInProductos")]
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("momento_degustacion_in_producto")]
    [Index("IdProducto", Name = "idProducto")]
    public partial class MomentoDegustacionInProducto
    {
        [Key]
        [Column("idMomentoDegustacion")]
        public int IdMomentoDegustacion { get; set; }
        [Key]
        [Column("idProducto")]
        public int IdProducto { get; set; }
        [Column("descripcion")]
        [StringLength(5)]
        [MySqlCollation("utf8_spanish_ci")]
        public string? Descripcion { get; set; }

        [ForeignKey("IdMomentoDegustacion")]
        [InverseProperty("MomentoDegustacionInProductos")]
        public virtual MomentoDegustacion IdMomentoDegustacionNavigation { get; set; } = null!;
        [ForeignKey("IdProducto")]
        [InverseProperty("MomentoDegustacionInProductos")]
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}

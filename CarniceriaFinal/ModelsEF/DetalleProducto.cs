using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("detalle_producto")]
    [Index("IdProducto", Name = "idDetalleProducto")]
    public partial class DetalleProducto
    {
        [Key]
        [Column("idDetalleProducto")]
        public int IdDetalleProducto { get; set; }
        [Column("tituloDetalle")]
        [StringLength(100)]
        public string TituloDetalle { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(10000)]
        public string Descripcion { get; set; } = null!;
        [Column("urlImg")]
        [StringLength(255)]
        public string? UrlImg { get; set; }
        [Column("idProducto")]
        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        [InverseProperty("DetalleProductos")]
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}

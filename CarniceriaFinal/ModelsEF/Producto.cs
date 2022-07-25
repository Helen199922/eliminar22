using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("producto")]
    [Index("IdUnidad", Name = "unidad_unidad")]
    public partial class Producto
    {
        public Producto()
        {
            DetalleProductos = new HashSet<DetalleProducto>();
            DetalleVenta = new HashSet<DetalleVentum>();
            MomentoDegustacionInProductos = new HashSet<MomentoDegustacionInProducto>();
            SubInCategoria = new HashSet<SubInCategorium>();
            IdPreparacionProductos = new HashSet<PreparacionProducto>();
        }

        [Key]
        [Column("idProducto")]
        public int IdProducto { get; set; }
        [Column("imgUrl")]
        [StringLength(255)]
        public string ImgUrl { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }
        [Column("precio")]
        public float? Precio { get; set; }
        [Column("titulo")]
        [StringLength(500)]
        public string Titulo { get; set; } = null!;
        [Column("status")]
        public int Status { get; set; }
        [Column("idUnidad")]
        public int IdUnidad { get; set; }
        [Column("stock")]
        public int Stock { get; set; }
        [Column("minimaUnidad")]
        [StringLength(5)]
        public string? MinimaUnidad { get; set; }

        [ForeignKey("IdUnidad")]
        [InverseProperty("Productos")]
        public virtual UnidadMedidum IdUnidadNavigation { get; set; } = null!;
        [InverseProperty("IdProductoNavigation")]
        public virtual ICollection<DetalleProducto> DetalleProductos { get; set; }
        [InverseProperty("IdProductoNavigation")]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
        [InverseProperty("IdProductoNavigation")]
        public virtual ICollection<MomentoDegustacionInProducto> MomentoDegustacionInProductos { get; set; }
        [InverseProperty("IdProductoNavigation")]
        public virtual ICollection<SubInCategorium> SubInCategoria { get; set; }

        [ForeignKey("IdProducto")]
        [InverseProperty("IdProductos")]
        public virtual ICollection<PreparacionProducto> IdPreparacionProductos { get; set; }
    }
}

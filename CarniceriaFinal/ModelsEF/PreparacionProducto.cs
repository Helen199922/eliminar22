using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("preparacion_producto")]
    public partial class PreparacionProducto
    {
        public PreparacionProducto()
        {
            MomentoDegustacionInPreparacions = new HashSet<MomentoDegustacionInPreparacion>();
            PreparacionProductoInProductos = new HashSet<PreparacionProductoInProducto>();
        }

        [Key]
        [Column("idPreparacionProducto")]
        public int IdPreparacionProducto { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;
        [Column("imagen")]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;
        [Column("status")]
        public int Status { get; set; }

        [InverseProperty("IdPreparacionProductoNavigation")]
        public virtual ICollection<MomentoDegustacionInPreparacion> MomentoDegustacionInPreparacions { get; set; }
        [InverseProperty("IdPreparacionProductoNavigation")]
        public virtual ICollection<PreparacionProductoInProducto> PreparacionProductoInProductos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("promocion")]
    public partial class Promocion
    {
        public Promocion()
        {
            DetalleVenta = new HashSet<DetalleVentum>();
            Productos = new HashSet<Producto>();
        }

        [Key]
        [Column("idPromocion")]
        public int IdPromocion { get; set; }
        [Column("fechaExpiracion", TypeName = "datetime")]
        public DateTime FechaExpiracion { get; set; }
        [Column("tipoPromo")]
        [StringLength(50)]
        public string TipoPromo { get; set; } = null!;
        [Column("porcentajePromo", TypeName = "double(10,0)")]
        public double PorcentajePromo { get; set; }

        [InverseProperty("IdPromocionNavigation")]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
        [InverseProperty("IdPromocionNavigation")]
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

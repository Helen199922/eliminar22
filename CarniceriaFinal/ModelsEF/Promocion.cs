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
            CorreoPromocions = new HashSet<CorreoPromocion>();
            DetalleVenta = new HashSet<DetalleVentum>();
            PromocionInProductos = new HashSet<PromocionInProducto>();
        }

        [Key]
        [Column("idPromocion")]
        public int IdPromocion { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("imagen")]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;
        [Column("fechaInicio", TypeName = "datetime")]
        public DateTime FechaInicio { get; set; }
        [Column("fechaFin", TypeName = "datetime")]
        public DateTime FechaFin { get; set; }
        [Column("maxParticipantes")]
        public int MaxParticipantes { get; set; }
        [Column("porcentajePromo", TypeName = "float(10,2)")]
        public float? PorcentajePromo { get; set; }
        [Column("dsctoMonetario")]
        public float? DsctoMonetario { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("fechaUpdate", TypeName = "datetime")]
        public DateTime FechaUpdate { get; set; }
        [Column("color")]
        [StringLength(255)]
        public string? Color { get; set; }

        [InverseProperty("IdPromocionNavigation")]
        public virtual ICollection<CorreoPromocion> CorreoPromocions { get; set; }
        [InverseProperty("IdPromocionNavigation")]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
        [InverseProperty("IdPromocionNavigation")]
        public virtual ICollection<PromocionInProducto> PromocionInProductos { get; set; }
    }
}

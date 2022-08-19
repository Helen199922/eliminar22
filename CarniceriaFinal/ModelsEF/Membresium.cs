using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("membresia")]
    public partial class Membresium
    {
        public Membresium()
        {
            MembresiaInUsuarios = new HashSet<MembresiaInUsuario>();
        }

        [Key]
        [Column("idMembresia")]
        public int IdMembresia { get; set; }
        [Column("porcentajeDescuento")]
        public float PorcentajeDescuento { get; set; }
        [Column("montoMaxAcceso")]
        public float? MontoMaxAcceso { get; set; }
        [Column("montoMinAcceso")]
        public float MontoMinAcceso { get; set; }
        [Column("montoMaxDescPorProducto")]
        public float MontoMaxDescPorProducto { get; set; }
        [Column("diasParaRenovar")]
        public int DiasParaRenovar { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("imagen")]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;
        [Column("duracionMembresiaDias")]
        public int DuracionMembresiaDias { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column("cantProductosMembresia")]
        public int? CantProductosMembresia { get; set; }

        [InverseProperty("IdMembresiaNavigation")]
        public virtual ICollection<MembresiaInUsuario> MembresiaInUsuarios { get; set; }
    }
}

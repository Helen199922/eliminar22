using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("modulo")]
    public partial class Modulo
    {
        public Modulo()
        {
            ModuloInOpcions = new HashSet<ModuloInOpcion>();
        }

        [Key]
        [Column("idModulo")]
        public int IdModulo { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string? Titulo { get; set; }
        [Column("icon")]
        [StringLength(30)]
        public string? Icon { get; set; } = null!;
        [Column("status")]
        public int? Status { get; set; }
        [Column("route")]
        [StringLength(30)]
        public string? Route { get; set; }

        [InverseProperty("IdModuloNavigation")]
        public virtual ICollection<ModuloInOpcion> ModuloInOpcions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("modulo_in_opcion")]
    [Index("IdOpcion", Name = "idOpcion")]
    public partial class ModuloInOpcion
    {
        [Key]
        [Column("idModulo")]
        public int IdModulo { get; set; }
        [Key]
        [Column("idOpcion")]
        public int IdOpcion { get; set; }
        [Column("descripcion")]
        [StringLength(5)]
        public string? Descripcion { get; set; }

        [ForeignKey("IdModulo")]
        [InverseProperty("ModuloInOpcions")]
        public virtual Modulo IdModuloNavigation { get; set; } = null!;
        [ForeignKey("IdOpcion")]
        [InverseProperty("ModuloInOpcions")]
        public virtual Opcion IdOpcionNavigation { get; set; } = null!;
    }
}

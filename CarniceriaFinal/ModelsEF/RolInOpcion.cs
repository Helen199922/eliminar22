using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("rol_in_opcion")]
    [Index("IdOpcion", Name = "idOpcion")]
    public partial class RolInOpcion
    {
        [Key]
        [Column("idRol")]
        public int IdRol { get; set; }
        [Key]
        [Column("idOpcion")]
        public int IdOpcion { get; set; }
        [Column("descripcion")]
        [StringLength(5)]
        public string? Descripcion { get; set; }

        [ForeignKey("IdOpcion")]
        [InverseProperty("RolInOpcions")]
        public virtual Opcion IdOpcionNavigation { get; set; } = null!;
        [ForeignKey("IdRol")]
        [InverseProperty("RolInOpcions")]
        public virtual Rol IdRolNavigation { get; set; } = null!;
    }
}

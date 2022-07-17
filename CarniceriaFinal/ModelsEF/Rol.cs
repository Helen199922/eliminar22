using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("rol")]
    public partial class Rol
    {
        public Rol()
        {
            RolInOpcions = new HashSet<RolInOpcion>();
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        [Column("idRol")]
        public int IdRol { get; set; }
        [Column("nombre")]
        [StringLength(25)]
        public string? Nombre { get; set; }
        [Column("descripcion")]
        [StringLength(60)]
        public string? Descripcion { get; set; }
        [Column("status")]
        public int? Status { get; set; }

        [InverseProperty("IdRolNavigation")]
        public virtual ICollection<RolInOpcion> RolInOpcions { get; set; }
        [InverseProperty("IdRolNavigation")]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("usuario")]
    [Index("IdPersona", Name = "personaUSR_personaUSR")]
    [Index("IdRol", Name = "rolUSR_rolUSR")]
    public partial class Usuario
    {
        [Key]
        [Column("idUsuario")]
        public int IdUsuario { get; set; }
        [Column("idRol")]
        public int? IdRol { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string? Username { get; set; }
        [Column("password")]
        [StringLength(255)]
        public string? Password { get; set; }
        [Column("idPersona")]
        public int? IdPersona { get; set; }
        [Column("status")]
        public int? Status { get; set; }

        [ForeignKey("IdPersona")]
        [InverseProperty("Usuarios")]
        public virtual Persona? IdPersonaNavigation { get; set; }
        [ForeignKey("IdRol")]
        [InverseProperty("Usuarios")]
        public virtual Rol? IdRolNavigation { get; set; }
    }
}

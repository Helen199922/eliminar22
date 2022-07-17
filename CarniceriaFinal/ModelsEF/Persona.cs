using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("persona")]
    [Index("IdCiudad", Name = "idCiudad")]
    [Index("IdSexo", Name = "idSexo")]
    public partial class Persona
    {
        public Persona()
        {
            Clientes = new HashSet<Cliente>();
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        [Column("idPersona")]
        public int IdPersona { get; set; }
        [Column("email")]
        [StringLength(200)]
        public string? Email { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        public string? Nombre { get; set; }
        [Column("apellido")]
        [StringLength(20)]
        public string? Apellido { get; set; }
        [Column("idSexo")]
        public int IdSexo { get; set; }
        [Column("cedula")]
        [StringLength(10)]
        public string? Cedula { get; set; }
        [Column("direccion1")]
        [StringLength(255)]
        public string? Direccion1 { get; set; }
        [Column("idCiudad")]
        public int? IdCiudad { get; set; }
        [Column("direccion2")]
        [StringLength(255)]
        public string? Direccion2 { get; set; }

        [ForeignKey("IdCiudad")]
        [InverseProperty("Personas")]
        public virtual Ciudad? IdCiudadNavigation { get; set; }
        [ForeignKey("IdSexo")]
        [InverseProperty("Personas")]
        public virtual Sexo IdSexoNavigation { get; set; } = null!;
        [InverseProperty("IdPersonaNavigation")]
        public virtual ICollection<Cliente> Clientes { get; set; }
        [InverseProperty("IdPersonaNavigation")]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}

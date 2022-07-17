using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("ciudad")]
    [Index("IdProvincia", Name = "provincia_provincia")]
    public partial class Ciudad
    {
        public Ciudad()
        {
            Clientes = new HashSet<Cliente>();
            Personas = new HashSet<Persona>();
            Venta = new HashSet<Ventum>();
        }

        [Column("ciudad")]
        [StringLength(20)]
        public string? Ciudad1 { get; set; }
        [Key]
        [Column("idCiudad")]
        public int IdCiudad { get; set; }
        [Column("idProvincia")]
        public int? IdProvincia { get; set; }
        [Column("costoEnvio")]
        public float? CostoEnvio { get; set; }

        [ForeignKey("IdProvincia")]
        [InverseProperty("Ciudads")]
        public virtual Provincium? IdProvinciaNavigation { get; set; }
        [InverseProperty("IdCiudadNavigation")]
        public virtual ICollection<Cliente> Clientes { get; set; }
        [InverseProperty("IdCiudadNavigation")]
        public virtual ICollection<Persona> Personas { get; set; }
        [InverseProperty("IdCiudadNavigation")]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}

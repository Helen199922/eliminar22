using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("opcion")]
    public partial class Opcion
    {
        public Opcion()
        {
            ModuloInOpcions = new HashSet<ModuloInOpcion>();
            OptionInEndpoints = new HashSet<OptionInEndpoint>();
            RolInOpcions = new HashSet<RolInOpcion>();
        }

        [Key]
        [Column("idOpcion")]
        public int IdOpcion { get; set; }
        [Column("titulo")]
        [StringLength(25)]
        public string? Titulo { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column("icon")]
        [StringLength(30)]
        public string? Icon { get; set; }
        [Column("route")]
        [StringLength(30)]
        public string? Route { get; set; }

        [InverseProperty("IdOpcionNavigation")]
        public virtual ICollection<ModuloInOpcion> ModuloInOpcions { get; set; }
        [InverseProperty("IdOptionNavigation")]
        public virtual ICollection<OptionInEndpoint> OptionInEndpoints { get; set; }
        [InverseProperty("IdOpcionNavigation")]
        public virtual ICollection<RolInOpcion> RolInOpcions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("sexo")]
    public partial class Sexo
    {
        public Sexo()
        {
            Personas = new HashSet<Persona>();
        }

        [Key]
        [Column("idSexo")]
        public int IdSexo { get; set; }
        [Column("sexo", TypeName = "tinytext")]
        public string? Sexo1 { get; set; }

        [InverseProperty("IdSexoNavigation")]
        public virtual ICollection<Persona> Personas { get; set; }
    }
}

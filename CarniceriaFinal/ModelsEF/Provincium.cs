using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("provincia")]
    public partial class Provincium
    {
        public Provincium()
        {
            Ciudads = new HashSet<Ciudad>();
        }

        [Column("provincia")]
        [StringLength(20)]
        public string? Provincia { get; set; }
        [Key]
        [Column("idProvincia")]
        public int IdProvincia { get; set; }

        [InverseProperty("IdProvinciaNavigation")]
        public virtual ICollection<Ciudad> Ciudads { get; set; }
    }
}

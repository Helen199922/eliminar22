using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("unidad_medida")]
    public partial class UnidadMedidum
    {
        public UnidadMedidum()
        {
            Productos = new HashSet<Producto>();
        }

        [Key]
        [Column("idUnidad")]
        public int IdUnidad { get; set; }
        [Column("unidad")]
        [StringLength(10)]
        public string Unidad { get; set; } = null!;

        [InverseProperty("IdUnidadNavigation")]
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

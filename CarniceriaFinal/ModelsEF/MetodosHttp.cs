using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("metodos_http")]
    [MySqlCollation("utf8_spanish_ci")]
    public partial class MetodosHttp
    {
        public MetodosHttp()
        {
            Endpoints = new HashSet<Endpoint>();
        }

        [Key]
        [Column("idMetodo")]
        public int IdMetodo { get; set; }
        [Column("tipoMetodo")]
        [StringLength(255)]
        public string? TipoMetodo { get; set; }

        [InverseProperty("IdMetodoNavigation")]
        public virtual ICollection<Endpoint> Endpoints { get; set; }
    }
}

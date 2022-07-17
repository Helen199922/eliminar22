using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("option_in_endpoint")]
    [Index("IdEndPoint", Name = "idEndPoint")]
    [MySqlCollation("utf8_spanish_ci")]
    public partial class OptionInEndpoint
    {
        [Key]
        [Column("idOption")]
        public int IdOption { get; set; }
        [Key]
        [Column("idEndPoint")]
        public int IdEndPoint { get; set; }
        [Column("descripcion")]
        [StringLength(5)]
        public string? Descripcion { get; set; }

        [ForeignKey("IdEndPoint")]
        [InverseProperty("OptionInEndpoints")]
        public virtual Endpoint IdEndPointNavigation { get; set; } = null!;
        [ForeignKey("IdOption")]
        [InverseProperty("OptionInEndpoints")]
        public virtual Opcion IdOptionNavigation { get; set; } = null!;
    }
}

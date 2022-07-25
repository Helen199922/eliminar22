using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("info_bancaria")]
    public partial class InfoBancarium
    {
        [Key]
        [Column("idBanco")]
        public int IdBanco { get; set; }
        [Column("tipoBanco")]
        [StringLength(255)]
        public string? TipoBanco { get; set; }
        [Column("numBanco")]
        [StringLength(255)]
        public string? NumBanco { get; set; }
        [Column("nombreBanco")]
        [StringLength(255)]
        public string? NombreBanco { get; set; }
    }
}

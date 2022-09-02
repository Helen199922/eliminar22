using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("porcentaje_dscto")]
    public partial class PorcentajeDscto
    {
        [Key]
        [Column("idPorcentajeDscto")]
        public int IdPorcentajeDscto { get; set; }
        [Column("porcentaje")]
        public float Porcentaje { get; set; }
    }
}

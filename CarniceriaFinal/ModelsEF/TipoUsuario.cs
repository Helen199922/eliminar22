using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("tipo_usuario")]
    public partial class TipoUsuario
    {
        [Key]
        [Column("idTipoUsuario")]
        public int IdTipoUsuario { get; set; }
        [Column("descripcion")]
        [StringLength(255)]
        public string? Descripcion { get; set; }
    }
}

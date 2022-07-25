using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("evento_especial")]
    public partial class EventoEspecial
    {
        [Key]
        [Column("idEventoEspecial")]
        public int IdEventoEspecial { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }
        [Column("imagen")]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;
        [Column("status")]
        public int Status { get; set; }
        [Column("fechaInicio", TypeName = "datetime")]
        public DateTime FechaInicio { get; set; }
        [Column("fechaFin", TypeName = "datetime")]
        public DateTime FechaFin { get; set; }
    }
}

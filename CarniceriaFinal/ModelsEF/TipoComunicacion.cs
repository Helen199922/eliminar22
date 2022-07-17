using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("tipo_comunicacion")]
    public partial class TipoComunicacion
    {
        public TipoComunicacion()
        {
            Comunicacions = new HashSet<Comunicacion>();
        }

        [Key]
        [Column("idTipoComunicacion")]
        public int IdTipoComunicacion { get; set; }
        [Column("titulo")]
        [StringLength(25)]
        public string? Titulo { get; set; }

        [InverseProperty("IdTipoComunicacionNavigation")]
        public virtual ICollection<Comunicacion> Comunicacions { get; set; }
    }
}

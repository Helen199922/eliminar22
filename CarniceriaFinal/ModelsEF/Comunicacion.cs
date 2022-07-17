using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("comunicacion")]
    [Index("IdTipoComunicacion", Name = "idTipoComunicacion")]
    public partial class Comunicacion
    {
        [Key]
        [Column("idComunicacion")]
        public int IdComunicacion { get; set; }
        [Column("tituloBoton")]
        [StringLength(25)]
        public string? TituloBoton { get; set; }
        [Column("titulo")]
        [StringLength(50)]
        public string? Titulo { get; set; }
        [Column("descripcion")]
        [StringLength(255)]
        public string? Descripcion { get; set; }
        [Column("idTipoComunicacion")]
        public int IdTipoComunicacion { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("fechaInicio", TypeName = "datetime")]
        public DateTime? FechaInicio { get; set; }
        [Column("fechaExpiracion", TypeName = "datetime")]
        public DateTime? FechaExpiracion { get; set; }
        [Column("urlImage")]
        [StringLength(255)]
        public string UrlImage { get; set; } = null!;
        [Column("urlBtn")]
        [StringLength(255)]
        public string? UrlBtn { get; set; }

        [ForeignKey("IdTipoComunicacion")]
        [InverseProperty("Comunicacions")]
        public virtual TipoComunicacion IdTipoComunicacionNavigation { get; set; } = null!;
    }
}

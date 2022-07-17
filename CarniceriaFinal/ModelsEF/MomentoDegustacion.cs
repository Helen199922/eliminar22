using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("momento_degustacion")]
    [MySqlCollation("utf8_spanish_ci")]
    public partial class MomentoDegustacion
    {
        public MomentoDegustacion()
        {
            MomentoDegustacionInProductos = new HashSet<MomentoDegustacionInProducto>();
        }

        [Key]
        [Column("idMomentoDegustacion")]
        public int IdMomentoDegustacion { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string? Titulo { get; set; }
        [Column("descripcion")]
        [StringLength(255)]
        public string? Descripcion { get; set; }
        [Column("urlImage")]
        [StringLength(255)]
        public string? UrlImage { get; set; }
        [Column("hora_inicio", TypeName = "time")]
        public TimeOnly? HoraInicio { get; set; }
        [Column("hora_fin", TypeName = "time")]
        public TimeOnly? HoraFin { get; set; }

        [InverseProperty("IdMomentoDegustacionNavigation")]
        public virtual ICollection<MomentoDegustacionInProducto> MomentoDegustacionInProductos { get; set; }
    }
}

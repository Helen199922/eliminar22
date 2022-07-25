using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("momento_degustacion")]
    public partial class MomentoDegustacion
    {
        public MomentoDegustacion()
        {
            MomentoDegustacionInPreparacions = new HashSet<MomentoDegustacionInPreparacion>();
            MomentoDegustacionInProductos = new HashSet<MomentoDegustacionInProducto>();
        }

        [Key]
        [Column("idMomentoDegustacion")]
        public int IdMomentoDegustacion { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;
        [Column("urlImage")]
        [StringLength(255)]
        public string UrlImage { get; set; } = null!;
        [Column("hora_inicio", TypeName = "datetime")]
        public DateTime? HoraInicio { get; set; }
        [Column("hora_fin", TypeName = "datetime")]
        public DateTime? HoraFin { get; set; }
        [Column("status")]
        public int Status { get; set; }

        [InverseProperty("IdMomentoDegustacionNavigation")]
        public virtual ICollection<MomentoDegustacionInPreparacion> MomentoDegustacionInPreparacions { get; set; }
        [InverseProperty("IdMomentoDegustacionNavigation")]
        public virtual ICollection<MomentoDegustacionInProducto> MomentoDegustacionInProductos { get; set; }
    }
}

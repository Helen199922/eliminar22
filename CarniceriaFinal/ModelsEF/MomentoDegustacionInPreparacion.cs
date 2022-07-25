using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("momento_degustacion_in_preparacion")]
    [Index("IdPreparacionProducto", Name = "idPreparacionProducto")]
    public partial class MomentoDegustacionInPreparacion
    {
        [Key]
        [Column("idMomentoDegustacion")]
        public int IdMomentoDegustacion { get; set; }
        [Key]
        [Column("idPreparacionProducto")]
        public int IdPreparacionProducto { get; set; }
        [Column("status")]
        public int Status { get; set; }

        [ForeignKey("IdMomentoDegustacion")]
        [InverseProperty("MomentoDegustacionInPreparacions")]
        public virtual MomentoDegustacion IdMomentoDegustacionNavigation { get; set; } = null!;
        [ForeignKey("IdPreparacionProducto")]
        [InverseProperty("MomentoDegustacionInPreparacions")]
        public virtual PreparacionProducto IdPreparacionProductoNavigation { get; set; } = null!;
    }
}

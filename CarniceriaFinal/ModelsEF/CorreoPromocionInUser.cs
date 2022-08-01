using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("correo_promocion_in_user")]
    [Index("IdCorreoPromocion", Name = "idCorreoPromocion")]
    [Index("IdEstatusEmail", Name = "idEstatusEmail")]
    [Index("IdUsuario", Name = "idUsuario")]
    public partial class CorreoPromocionInUser
    {
        [Key]
        [Column("idCorreoPromocionInUser")]
        public int IdCorreoPromocionInUser { get; set; }
        [Column("idUsuario")]
        public int IdUsuario { get; set; }
        [Column("idEstatusEmail")]
        public int IdEstatusEmail { get; set; }
        [Column("idCorreoPromocion")]
        public int? IdCorreoPromocion { get; set; }
        [Column("fechaUpdate", TypeName = "datetime")]
        public DateTime FechaUpdate { get; set; }
        [Column("fechaSender", TypeName = "datetime")]
        public DateTime FechaSender { get; set; }

        [ForeignKey("IdCorreoPromocion")]
        [InverseProperty("CorreoPromocionInUsers")]
        public virtual CorreoPromocion? IdCorreoPromocionNavigation { get; set; }
        [ForeignKey("IdEstatusEmail")]
        [InverseProperty("CorreoPromocionInUsers")]
        public virtual StatusEmail IdEstatusEmailNavigation { get; set; } = null!;
        [ForeignKey("IdUsuario")]
        [InverseProperty("CorreoPromocionInUsers")]
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("correo_promocion")]
    [Index("IdPromocion", Name = "idPromocion")]
    public partial class CorreoPromocion
    {
        public CorreoPromocion()
        {
            CorreoPromocionInUsers = new HashSet<CorreoPromocionInUser>();
        }

        [Key]
        [Column("idCorreo")]
        public int IdCorreo { get; set; }
        [Column("imagen")]
        [StringLength(255)]
        public string Imagen { get; set; } = null!;
        [Column("urlPromocion")]
        [StringLength(255)]
        public string UrlPromocion { get; set; } = null!;
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = null!;
        [Column("descripcion")]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;
        [Column("fechaUpdate", TypeName = "datetime")]
        public DateTime FechaUpdate { get; set; }
        [Column("idPromocion")]
        public int IdPromocion { get; set; }
        [Column("isSendingEmails")]
        public int IsSendingEmails { get; set; }

        [ForeignKey("IdPromocion")]
        [InverseProperty("CorreoPromocions")]
        public virtual Promocion IdPromocionNavigation { get; set; } = null!;
        [InverseProperty("IdCorreoPromocionNavigation")]
        public virtual ICollection<CorreoPromocionInUser> CorreoPromocionInUsers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("status_email")]
    public partial class StatusEmail
    {
        public StatusEmail()
        {
            CorreoPromocionInUsers = new HashSet<CorreoPromocionInUser>();
        }

        [Key]
        [Column("idEstatusEmail")]
        public int IdEstatusEmail { get; set; }
        [Column("titulo")]
        [StringLength(10)]
        public string? Titulo { get; set; }

        [InverseProperty("IdEstatusEmailNavigation")]
        public virtual ICollection<CorreoPromocionInUser> CorreoPromocionInUsers { get; set; }
    }
}

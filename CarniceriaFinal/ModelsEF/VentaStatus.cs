using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("venta_status")]
    public partial class VentaStatus
    {
        public VentaStatus()
        {
            Venta = new HashSet<Ventum>();
        }

        [Key]
        [Column("idVentaStatus")]
        public int IdVentaStatus { get; set; }
        [Column("status")]
        [StringLength(15)]
        public string? Status { get; set; }
        [Column("descripcion")]
        [StringLength(255)]
        public string? Descripcion { get; set; }

        [InverseProperty("IdStatusNavigation")]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("forma_pago")]
    public partial class FormaPago
    {
        public FormaPago()
        {
            Venta = new HashSet<Ventum>();
        }

        [Key]
        [Column("idFormaPago")]
        public int IdFormaPago { get; set; }
        [Column("tipoFormaPago")]
        [StringLength(50)]
        public string? TipoFormaPago { get; set; }
        [Column("descripcion")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [InverseProperty("IdFormaPagoNavigation")]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}

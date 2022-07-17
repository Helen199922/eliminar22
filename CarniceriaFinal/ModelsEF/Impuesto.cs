using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("impuesto")]
    public partial class Impuesto
    {
        public Impuesto()
        {
            Venta = new HashSet<Ventum>();
        }

        [Key]
        [Column("idImpuesto")]
        public int IdImpuesto { get; set; }
        [Column("porcentaje")]
        public float Porcentaje { get; set; }
        [Column("fechaExpiracion", TypeName = "datetime")]
        public DateTime? FechaExpiracion { get; set; }

        [InverseProperty("IdImpuestoNavigation")]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("membresia_in_usuario")]
    [Index("IdMembresia", Name = "idMembresia")]
    [Index("IdUsuario", Name = "idUsuario")]
    public partial class MembresiaInUsuario
    {
        public MembresiaInUsuario()
        {
            DetalleVenta = new HashSet<DetalleVentum>();
        }

        [Key]
        [Column("idMembresiaInUsuario")]
        public int IdMembresiaInUsuario { get; set; }
        [Column("idUsuario")]
        public int IdUsuario { get; set; }
        [Column("idMembresia")]
        public int IdMembresia { get; set; }
        [Column("fechaFin", TypeName = "datetime")]
        public DateTime FechaFin { get; set; }
        [Column("fechaInicio", TypeName = "datetime")]
        public DateTime FechaInicio { get; set; }
        [Column("cantProductosComprados")]
        public int CantProductosComprados { get; set; }
        [Column("status")]
        public int Status { get; set; }

        [ForeignKey("IdMembresia")]
        [InverseProperty("MembresiaInUsuarios")]
        public virtual Membresium IdMembresiaNavigation { get; set; } = null!;
        [ForeignKey("IdUsuario")]
        [InverseProperty("MembresiaInUsuarios")]
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
        [InverseProperty("IdMembresiaInUsuarioNavigation")]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
    }
}

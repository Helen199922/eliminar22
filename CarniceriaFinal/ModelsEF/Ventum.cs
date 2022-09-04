using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("venta")]
    [Index("IdCliente", Name = "cliente_cliente")]
    [Index("IdAdm", Name = "idAdm")]
    [Index("IdCiudad", Name = "idCiudad")]
    [Index("IdFormaPago", Name = "idFormaPago")]
    [Index("IdStatus", Name = "idStatus")]
    [Index("IdImpuesto", Name = "impuesto_impuesto")]
    public partial class Ventum
    {
        public Ventum()
        {
            DetalleVenta = new HashSet<DetalleVentum>();
        }

        [Key]
        [Column("idVenta")]
        public int IdVenta { get; set; }
        [Column("idCliente")]
        public int? IdCliente { get; set; }
        [Column("fecha", TypeName = "datetime")]
        public DateTime? Fecha { get; set; }
        [Column("idImpuesto")]
        public int? IdImpuesto { get; set; }
        [Column("total")]
        public float? Total { get; set; }
        [Column("costosAdicionales")]
        public float? CostosAdicionales { get; set; }
        [Column("motivoCostosAdicional")]
        [StringLength(100)]
        public string? MotivoCostosAdicional { get; set; }
        [Column("idFormaPago")]
        public int? IdFormaPago { get; set; }
        [Column("direccion")]
        [StringLength(255)]
        public string? Direccion { get; set; }
        [Column("referencia")]
        [StringLength(255)]
        public string? Referencia { get; set; }
        [Column("idCiudad")]
        public int? IdCiudad { get; set; }
        [Column("idStatus")]
        public int? IdStatus { get; set; }
        [Column("fechaFinal", TypeName = "datetime")]
        public DateTime? FechaFinal { get; set; }
        [Column("isAuthUser")]
        public int? IsAuthUser { get; set; }
        [Column("idAdm")]
        public int? IdAdm { get; set; }

        [ForeignKey("IdAdm")]
        [InverseProperty("Venta")]
        public virtual Usuario? IdAdmNavigation { get; set; }
        [ForeignKey("IdCiudad")]
        [InverseProperty("Venta")]
        public virtual Ciudad? IdCiudadNavigation { get; set; }
        [ForeignKey("IdCliente")]
        [InverseProperty("Venta")]
        public virtual Cliente? IdClienteNavigation { get; set; }
        [ForeignKey("IdFormaPago")]
        [InverseProperty("Venta")]
        public virtual FormaPago? IdFormaPagoNavigation { get; set; }
        [ForeignKey("IdImpuesto")]
        [InverseProperty("Venta")]
        public virtual Impuesto? IdImpuestoNavigation { get; set; }
        [ForeignKey("IdStatus")]
        [InverseProperty("Venta")]
        public virtual VentaStatus? IdStatusNavigation { get; set; }
        [InverseProperty("IdVentaNavigation")]
        public virtual ICollection<DetalleVentum> DetalleVenta { get; set; }
    }
}

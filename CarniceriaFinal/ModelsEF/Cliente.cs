using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("cliente")]
    [Index("IdCiudad", Name = "ciudad_ciudad")]
    [Index("IdPersona", Name = "persona_persona")]
    public partial class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Ventum>();
        }

        [Key]
        [Column("idCliente")]
        public int IdCliente { get; set; }
        [Column("direccion")]
        [StringLength(255)]
        public string? Direccion { get; set; }
        [Column("referencia")]
        [StringLength(300)]
        public string? Referencia { get; set; }
        [Column("idCiudad")]
        public int? IdCiudad { get; set; }
        [Column("telefono1")]
        [StringLength(15)]
        public string? Telefono1 { get; set; }
        [Column("telefono2")]
        [StringLength(15)]
        public string? Telefono2 { get; set; }
        [Column("idPersona")]
        public int? IdPersona { get; set; }

        [ForeignKey("IdCiudad")]
        [InverseProperty("Clientes")]
        public virtual Ciudad? IdCiudadNavigation { get; set; }
        [ForeignKey("IdPersona")]
        [InverseProperty("Clientes")]
        public virtual Persona? IdPersonaNavigation { get; set; }
        [InverseProperty("IdClienteNavigation")]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}

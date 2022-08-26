using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("categoria_in_producto")]
    [Index("IdCategoria", Name = "idCategoria")]
    [Index("IdProducto", Name = "idProducto")]
    public partial class CategoriaInProducto
    {
        [Key]
        [Column("idCategoriaInProduct")]
        public int IdCategoriaInProduct { get; set; }
        [Column("idCategoria")]
        public int? IdCategoria { get; set; }
        [Column("idProducto")]
        public int? IdProducto { get; set; }

        [ForeignKey("IdCategoria")]
        [InverseProperty("CategoriaInProductos")]
        public virtual CategoriaProducto? IdCategoriaNavigation { get; set; }
        [ForeignKey("IdProducto")]
        [InverseProperty("CategoriaInProductos")]
        public virtual Producto? IdProductoNavigation { get; set; }
    }
}

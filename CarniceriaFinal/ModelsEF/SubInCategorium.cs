using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("sub_in_categoria")]
    [Index("IdCategoria", Name = "idCategoria")]
    [Index("IdProducto", Name = "idProducto")]
    [Index("IdSubCategoria", Name = "idSubCategoria")]
    [MySqlCollation("utf8_spanish_ci")]
    public partial class SubInCategorium
    {
        [Key]
        [Column("idSubInCategory")]
        public int IdSubInCategory { get; set; }
        [Column("idCategoria")]
        public int IdCategoria { get; set; }
        [Column("idSubCategoria")]
        public int IdSubCategoria { get; set; }
        [Column("idProducto")]
        public int? IdProducto { get; set; }

        [ForeignKey("IdCategoria")]
        [InverseProperty("SubInCategoria")]
        public virtual CategoriaProducto IdCategoriaNavigation { get; set; } = null!;
        [ForeignKey("IdProducto")]
        [InverseProperty("SubInCategoria")]
        public virtual Producto? IdProductoNavigation { get; set; }
        [ForeignKey("IdSubCategoria")]
        [InverseProperty("SubInCategoria")]
        public virtual SubCategorium IdSubCategoriaNavigation { get; set; } = null!;
    }
}

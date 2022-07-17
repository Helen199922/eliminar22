using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("categoria_producto")]
    public partial class CategoriaProducto
    {
        public CategoriaProducto()
        {
            SubInCategoria = new HashSet<SubInCategorium>();
        }

        [Key]
        [Column("idCategoria")]
        public int IdCategoria { get; set; }
        [Column("titulo")]
        [StringLength(70)]
        public string? Titulo { get; set; }
        [Column("descripcion")]
        [StringLength(150)]
        public string? Descripcion { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("urlImage")]
        [StringLength(255)]
        public string? UrlImage { get; set; }

        [InverseProperty("IdCategoriaNavigation")]
        public virtual ICollection<SubInCategorium> SubInCategoria { get; set; }
    }
}

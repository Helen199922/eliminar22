using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("sub_categoria")]
    public partial class SubCategorium
    {
        public SubCategorium()
        {
            SubInCategoria = new HashSet<SubInCategorium>();
        }

        [Key]
        [Column("idSubCategoria")]
        public int IdSubCategoria { get; set; }
        [Column("titulo")]
        [StringLength(255)]
        public string? Titulo { get; set; }
        [Column("descripcion")]
        [StringLength(700)]
        public string? Descripcion { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [StringLength(255)]
        public string? UrlImage { get; set; }

        [InverseProperty("IdSubCategoriaNavigation")]
        public virtual ICollection<SubInCategorium> SubInCategoria { get; set; }
    }
}

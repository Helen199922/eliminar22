using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("endpoints")]
    [Index("IdMetodo", Name = "idMetodo")]
    public partial class Endpoint
    {
        public Endpoint()
        {
            OptionInEndpoints = new HashSet<OptionInEndpoint>();
        }

        [Key]
        [Column("idEndPoint")]
        public int IdEndPoint { get; set; }
        [Column("endPoint")]
        [StringLength(255)]
        public string? EndPoint1 { get; set; }
        [Column("pathEndpoint")]
        [StringLength(255)]
        public string? PathEndpoint { get; set; }
        [Column("isPublic")]
        public int IsPublic { get; set; }
        [Column("idMetodo")]
        public int? IdMetodo { get; set; }

        [ForeignKey("IdMetodo")]
        [InverseProperty("Endpoints")]
        public virtual MetodosHttp? IdMetodoNavigation { get; set; }
        [InverseProperty("IdEndPointNavigation")]
        public virtual ICollection<OptionInEndpoint> OptionInEndpoints { get; set; }
    }
}

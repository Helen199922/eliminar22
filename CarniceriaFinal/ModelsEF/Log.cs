using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("logs")]
    public partial class Log
    {
        [Key]
        [Column("idLog")]
        public int IdLog { get; set; }
        [Column("modulo")]
        [StringLength(255)]
        public string Modulo { get; set; } = null!;
        [Column("metodo")]
        [StringLength(255)]
        public string Metodo { get; set; } = null!;
        [Column("idModulo")]
        public int IdModulo { get; set; }
        [Column("mensaje")]
        [StringLength(255)]
        public string Mensaje { get; set; } = null!;
        [Column("estadoHTTP")]
        [StringLength(255)]
        public string EstadoHttp { get; set; } = null!;
        [Column("timestamp", TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        [Column("hostname")]
        [StringLength(255)]
        public string Hostname { get; set; } = null!;
        [Column("endpoint")]
        [StringLength(255)]
        public string Endpoint { get; set; } = null!;
        [Column("pathEndpoint")]
        [StringLength(255)]
        public string? PathEndpoint { get; set; }
        [Column("idUser")]
        [StringLength(255)]
        public string? IdUser { get; set; }
    }
}

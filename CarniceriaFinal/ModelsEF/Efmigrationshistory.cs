using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.ModelsEF
{
    [Table("__efmigrationshistory")]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public partial class Efmigrationshistory
    {
        [Key]
        [StringLength(150)]
        public string MigrationId { get; set; } = null!;
        [StringLength(32)]
        public string ProductVersion { get; set; } = null!;
    }
}

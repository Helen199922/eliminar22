using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.DTOs
{
    public class PromotionEntity
    {
        public int IdPromocion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public string? TipoPromo { get; set; }
        public double PorcentajePromo { get; set; }
    }
}

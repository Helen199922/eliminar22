using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.DTOs
{
    public class PromotionEntity
    {
        public int idPromocion { get; set; }
        public string titulo { get; set; }
        public string imagen { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
    }
}

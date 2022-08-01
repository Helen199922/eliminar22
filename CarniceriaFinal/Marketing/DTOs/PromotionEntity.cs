using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.DTOs
{
    public class PromotionSimpleEntity
    {
        public int idPromocion { get; set; }
        public string titulo { get; set; }
        public string imagen { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
    }
    public class PromotionEntity
    {
        public int? idPromocion { get; set; }
        public string titulo { get; set; }
        public string imagen { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int MaxParticipantes { get; set; }
        public float? PorcentajePromo { get; set; }
        public float? DsctoMonetario { get; set; }
        public int? Status { get; set; }
        public DateTime? FechaUpdate { get; set; }
    }
}

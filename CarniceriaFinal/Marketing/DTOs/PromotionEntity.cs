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
        public List<int> pruductsInPromo { get; set; }
    }
    public class PromotionProductEntity
    {
        public int idProduct { get; set; }
        public string titulo { get; set; }
        public int stock { get; set; }
        public Boolean isActivate { get; set; }
    }
    public class PromotionProductRequestEntity
    {
        public int? idPromotion { get; set; }
    }
}

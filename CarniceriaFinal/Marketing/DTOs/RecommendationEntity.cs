using CarniceriaFinal.Core.TimesConverts;
using System.Text.Json.Serialization;

namespace CarniceriaFinal.Marketing.DTOs
{
    public class AdmStatusRecommendation
    {
        public int id { get; set; }
        public Boolean status { get; set; }
    }
    public class CreatePreparationWay : PreparationWay
    {
        public List<int> productIds { get; set; }
    }
    public class CreateTimesToEatWay : TimesToEatEntity
    {
        public List<int> preparations { get; set; }
    }
    public class TimesToEatEntity
    {
        public int IdMomentoDegustacion { get; set; }
        public int status { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlImage { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly? HoraInicio { get; set; }
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly? HoraFin { get; set; }
    }
    public class PreparationWay
    {
        public int IdPreparacionProducto { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public int? Status { get; set; }
    }
}

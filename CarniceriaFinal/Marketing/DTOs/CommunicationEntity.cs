using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.DTOs
{
    public class CommunicationEntity
    {
        public int IdComunicacion { get; set; }
        public string? TituloBoton { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int IdTipoComunicacion { get; set; }
        public int Status { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public string? UrlImage { get; set; }
        public TypeCommunicationEntity? TipoComunicacion { get; set; }
        public string? urlBtn { get; set; }

    }
}

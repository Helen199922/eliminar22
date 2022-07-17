using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.User.DTOs
{
    public class PersonEntity
    {
        public int? idPersona { get; set; }
        public string? email {get; set;}
        public string? nombre {get; set;}
        public string? apellido {get; set;}
        public int idSexo {get; set;}
        public string? cedula {get; set;}
        public string? direccion1 { get; set; }
        public string? direccion2 { get; set; }
        public string? idCiudad { get; set; }
    }
}

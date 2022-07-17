using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.User.DTOs
{
    public class PruebaPerson
    {
        public int IdPersona { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdSexo { get; set; }
        public string Cedula { get; set; }
    }
}

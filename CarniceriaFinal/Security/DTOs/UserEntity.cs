using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Cliente.Models
{
    public class UserEntity
    {
        public int? idUsuario { get; set; }
        public int? idRol { get; set; }
        public string? username {get; set;}
        public string? password {get; set;}
        public int idPersona {get; set;}
        public int status { get; set; }
        public string? rolName { get; set; }
        public int? ReceiveEmail { get; set; }
    }
}

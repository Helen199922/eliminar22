using CarniceriaFinal.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.DTOs
{
    public class UserAuthResponseEntity
    {
        public string? token { get; set; }
        public string? username { get; set; }
        public int idRol { get; set; }
        public int idUser { get; set; }
        public Boolean isAdminUser { get; set; }
        public List<MenuEntity> menu { get; set; }
    }
}

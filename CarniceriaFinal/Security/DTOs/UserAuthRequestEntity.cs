using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.DTOs
{
    public class UserAuthRequestEntity
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}

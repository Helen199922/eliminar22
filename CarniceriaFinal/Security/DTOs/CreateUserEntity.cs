using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.DTOs
{
    public class CreateUserEntity : PersonEntity
    {
        public string username { get; set; }
        public string password { get; set; }

    }
}

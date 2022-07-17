using CarniceriaFinal.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.User.DTOs
{
    public class DetailUserEntity
    {
        public PersonEntity persona { get; set; }
        public UserEntity usuario { get; set; }
    }
}

using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Cliente.Models
{
    public class ClientEntity : PersonEntity
    {
        public int idCliente {get; set;}
        public string? direccion {get; set;}
        public string? referencia {get; set;}
        public int idCiudad {get; set;}
        public string? telefono1 {get; set;}
        public string? telefono2 {get; set;}
    }
}

using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Security.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.Services.IServices
{
    public interface ILoginService
    {
        Task<UserAuthResponseEntity> Login(UserAuthRequestEntity user);
        Task<List<MenuEntity>> Menu(int idRol);
    }
}

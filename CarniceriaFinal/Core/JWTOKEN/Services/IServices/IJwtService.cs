using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.JWTOKEN.Services.IServices
{
    public interface IJwtService
    {
        Task<UserEntity> GetUserById(int idUser);
        Task<Boolean> IsPublicEndPoint(string endPoint, string method);
        Task<Boolean> FindOptionByIdRolAndMethodAndEndPoint(int idRol, string endPoint, string method);
    }
}

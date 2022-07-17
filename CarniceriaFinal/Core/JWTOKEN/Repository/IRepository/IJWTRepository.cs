using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.JWTOKEN.Repository.IRepository
{
    public interface IJWTRepository
    {
        Opcion FindOptionByIdRolAndMethodAndEndPoint(int idRol, string endPoint, string method);
        Task<Boolean> IsPublicEndPoint(string endPoint, string method);
        Task<Usuario> GetUserById(int idUser);
    }
}

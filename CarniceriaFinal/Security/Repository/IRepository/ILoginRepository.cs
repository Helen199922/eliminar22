using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.Repository.IRepository
{
    public interface ILoginRepository
    {
        Task<Usuario> GetUserByUserName(string username, string password);
        Task<List<ModuloInOpcion>> GetMenuByIdRol(int idRol);
    }
}

using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.IRepository
{
    public interface IUserRepository
    {
        Task<Usuario> GetUserByIdIndentificationPerson(string indetification);
        Task<Usuario> GetUserRolByIdIndentificationPerson(string indetification);
        Task<Usuario> GetUserByUserName(string username);
        Task<Usuario> GetPersonByUserName(string username);
        Task<Usuario> GetUserByUserNameAndIdUser(string username, int? idUser = 0);
        Task<List<Usuario>> GetAllUser();
        Task<Usuario> CreateUser(Usuario user);
    }
}

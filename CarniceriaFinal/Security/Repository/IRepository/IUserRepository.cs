using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.DTOs;
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
        Task<Usuario> GetUserByUserNameOrEmail(string username, string email, int idUser);
        Task<Usuario> GetUserByUserNameAndIdUser(string username, int? idUser = 0);
        Task<List<Usuario>> GetAllUser();
        Task<Usuario> CreateUser(Usuario user);
        Task<Usuario> GetProfileInfo(int idUser);
        Task<Usuario> UpdateProfileInfo(ProfileUserEntity profile, int idUser);
        Task<Usuario> GetUserByEmail(string email);
    }
}

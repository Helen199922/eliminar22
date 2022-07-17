using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.Services.IServices
{
    public interface IUserService
    {
        Task<List<DetailUserEntity>> GetAllUsers();
        Task<UserAuthResponseEntity> CreateUser(DetailUserEntity user);
        Task<UserEntity> CreateUserByRol(DetailUserEntity user);
        Task<PersonEntity> GetPersonByUserName(string username);
        Task<PersonEntity> ManagementPerson(PersonEntity person);
    }
}

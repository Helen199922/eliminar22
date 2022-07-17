using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Roles.DTOs;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Roles.Services.IServices
{
    public interface IRolService
    {
        Task<RolEntity> GetRolByIdUser(int idUser);
        Task<List<DetailRole>> GetAllDetailRoles();
        Task<List<RolOptionModules>> GetOptionsByRoles(int idRol);
        Task<List<RolEntity>> GetAllRoles();
        Task<Boolean> DisabledRol(int idRol);
        Task<Boolean> EnableRol(int idRol);
        Task<List<RolOptionModules>> UpdateRolPermissions(int idRol, List<RolOptionModules> options);
        Task<UserEntity> CreateUserAdminByRol(DetailUserEntity user);
        Task<DetailUserEntity> UpdateUserAdminByRol(DetailUserEntity user);
        Task<List<DetailUserEntity>> GetAllUsersAdmin();
        Task<Boolean> DisabledUser(int idUser);
        Task<Boolean> EnableUser(int idUser);
    }
}

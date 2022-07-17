using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Roles.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Roles.Repository.IRepository
{
    public interface IRolRepository
    {
        Task<Rol> GetRolByIdUser(int idUser);
        Task<List<DetailRole>> GetAllDetailRoles();
        Task<List<RolOptionModules>> GetOptionsByRoles(int idRol);
        Task<Boolean> DisabledRol(int idRol);
        Task<Boolean> EnableRol(int idRol);
        Task<List<RolInOpcion>> UpdateRolPermissions(int idRol, List<RolOptionModules> modules);
        Task<Usuario> UpdateUserAdminByRol(Usuario user);
        Task<List<Usuario>> GetAllUserAdminWithRol();
        Task<Boolean> DisabledUser(int idUser);
        Task<Boolean> EnableUser(int idUser);
        Task<List<Rol>> GetAllRoles();
    }
}

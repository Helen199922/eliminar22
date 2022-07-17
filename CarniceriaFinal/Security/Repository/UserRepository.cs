using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly DBContext Context;
        public UserRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<Usuario> GetUserByUserName(string username)
        {
            try
            {
                return await Context.Usuarios.Where(x =>
                x.Username.ToLower() == username.ToLower())
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por userName");
            }
        }

        public async Task<Usuario> GetUserByUserNameAndIdUser(string username, int? idUser = 0)
        {
            try
            {
                return await Context.Usuarios.Where(x =>
                x.Username.ToLower() == username.ToLower() && x.IdUsuario == idUser)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por userName");
            }
        }

        public async Task<Usuario> GetPersonByUserName(string username)
        {
            try
            {
                return await Context.Usuarios
                    .Include(x => x.IdPersonaNavigation)
                    .Where(x => x.Username.ToLower() == username.ToLower())
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por userName");
            }
        }

        public async Task<Usuario> GetUserByIdIndentificationPerson(string indetification)
        {
            try
            {
                return await Context.Usuarios.Where(x => 
                (x.IdPersonaNavigation.Cedula == indetification) && x.IdRol == 3 )
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por número de identificación");
            }
        }
        public async Task<Usuario> GetUserRolByIdIndentificationPerson(string indetification)
        {
            try
            {
                return await Context.Usuarios.Where(x =>
                (x.IdPersonaNavigation.Cedula == indetification) && x.IdRol != 3)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por número de identificación");
            }
        }
        public async Task<Usuario> CreateUser(Usuario user)
        {
            try
            {
                user.Status = 1;
                await Context.Usuarios.AddAsync(user);
                await Context.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear usuario");
            }
        }
        public async Task<List<Usuario>> GetAllUser()
        {
            try
            {
                return await Context.Usuarios.Include(x => x.IdPersonaNavigation).ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener todos los usuario");
            }
        }
    }
}

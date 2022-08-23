using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.DTOs;
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
        public async Task<Usuario> GetUserByUserNameOrEmail(string username, string email, int idUser)
        {
            try
            {
                return await Context.Usuarios.Where(x =>
                ((x.Username.ToLower() == username.ToLower() ||
                    x.IdPersonaNavigation.Email.ToLower() == email.ToLower())
                    && (x.IdUsuario != idUser)
                )).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por userName e email");
            }
        }
        public async Task<Usuario> GetUserByEmail(string email)
        {
            try
            {
                return await Context.Usuarios.Where(x =>
                x.IdPersonaNavigation.Email.ToLower() == email.ToLower()
                && x.IdRol == 3
                ).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Buscar usuario por email");
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
        public async Task<Usuario> GetPersonByIdUser(int idUser)
        {
            try
            {
                return await Context.Usuarios
                    .Include(x => x.IdPersonaNavigation)
                    .Where(x => x.IdUsuario == idUser)
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
        public async Task<Usuario> GetProfileInfo(int idUser)
        {
            try
            {
                return await Context.Usuarios
                    .Include(x => x.IdPersonaNavigation)
                    .Where(x => (x.IdUsuario == idUser && x.Status == 1 && x.IdRol == 3))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener todos los usuario");
            }
        }
        public async Task<Usuario> UpdateProfileInfo(ProfileUserEntity profile, int idUser)
        {
            try
            {
                var profileValue = await Context.Usuarios
                    .Include(x => x.IdPersonaNavigation)
                    .Where(x => (x.IdUsuario == idUser && x.Status == 1 && x.IdRol == 3))
                    .FirstOrDefaultAsync();


                profileValue.Username = profile.username;
                profileValue.Password = profile.password;
                profileValue.PerfilImage = profile.profileImage;
                profileValue.IdPersonaNavigation.Email = profile.email;
                profileValue.IdPersonaNavigation.Nombre = profile.nombre;
                profileValue.IdPersonaNavigation.Apellido = profile.apellido;
                profileValue.IdPersonaNavigation.Direccion1 = profile.direccion;

                await Context.SaveChangesAsync();
                return profileValue;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar el usuario");
            }
        }
    }
}

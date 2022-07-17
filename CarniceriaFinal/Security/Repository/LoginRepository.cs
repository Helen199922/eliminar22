using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Repository.IRepository;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public readonly DBContext Context;
        public LoginRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<Usuario> GetUserByUserName(string username, string password)
        {
            try
            {
                var user = await Context.Usuarios
                    .Include(x => x.IdRolNavigation)
                    .Where(x => x.Username == username && x.Password == password)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Usuario");
            }
        }
        public async Task<List<ModuloInOpcion>> GetMenuByIdRol(int idRol)
        {
            try
            {
                //GET the options by idRol
                var list = await Context.ModuloInOpcions
                    .Include(x => x.IdModuloNavigation)
                    .Include(x => x.IdOpcionNavigation)
                    .Where(x => x.IdOpcionNavigation.RolInOpcions.Any(y => y.IdRol == idRol))
                    .ToListAsync();

                return list;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Opciones");
            }
        }
    }
}

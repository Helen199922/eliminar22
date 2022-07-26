using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.JWTOKEN.Repository
{
    public class JWTRepository : IJWTRepository
    {
        public readonly DBContext Context;
        public JWTRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public Opcion FindOptionByIdRolAndMethodAndEndPoint(int idRol, string endPoint, string method)
        {
            try
            {

                using (var _Context = new DBContext())
                {
                    var option = _Context.Opcions
                    .FromSqlRaw("call sp_option_by_endpoint_method_rol ({0}, {1}, {2})", idRol, endPoint, method)
                    .AsEnumerable().Select(x => x.IdOpcion).ToList();

                    if (option.Count == 0) return null;
                }
                return new();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("opciones");
            }
        }
        public async Task<Boolean> IsPublicEndPoint(string endPoint, string method)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    ModelsEF.Endpoint option = await _Context.Endpoints
                    .Include(x => x.IdMetodoNavigation)
                    .Where(x => x.EndPoint1 == endPoint && x.IdMetodoNavigation.TipoMetodo == method)
                    .FirstOrDefaultAsync();

                    if (option == null) return false;

                    return option.IsPublic.Equals(1);
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Opcion");
            }
        }
        public async Task<Boolean> IsOnlyForUSer(string endPoint, string method)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    ModelsEF.Endpoint option = await _Context.Endpoints
                    .Include(x => x.IdMetodoNavigation)
                    .Where(x => x.EndPoint1 == endPoint && x.IdMetodoNavigation.TipoMetodo == method)
                    .FirstOrDefaultAsync();

                    if (option == null) return false;

                    return option.IsPublic.Equals(3);
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Opcion");
            }
        }
        
        public async Task<Usuario> GetUserById(int idUser)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    return await _Context.Usuarios
                    .Where(x => x.IdUsuario == idUser)
                    .FirstOrDefaultAsync();
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Usuario By Id");
            }
        }
    }
}

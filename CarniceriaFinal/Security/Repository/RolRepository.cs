using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Roles.DTOs;
using CarniceriaFinal.Roles.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Roles.Repository
{
    public class RolRepository : IRolRepository
    {
        public readonly DBContext Context;
        public RolRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<Rol> GetRolByIdUser(int idUser)
        {
            try
            {
                var rol = await Context.Usuarios
                    .Include(x => x.IdRolNavigation)
                    .Where(x => x.IdRol == idUser)
                    .FirstOrDefaultAsync();

                return rol.IdRolNavigation;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Roles");
            }
        }
        public async Task<List<DetailRole>> GetAllDetailRoles()
        {
            try
            {
                    List<DetailRole> roles = new();
                using (var _Context = new DBContext())
                {

                    var RelationatedRoles = await _Context.RolInOpcions
                        .Include(x => x.IdOpcionNavigation)
                        .Include(x => x.IdRolNavigation.Usuarios)
                        .AsNoTracking()
                        .ToListAsync();

                    var AllRoles = await _Context.Rols
                        .AsNoTracking()
                        .Where(x => x.IdRol != 3)
                        .ToListAsync();

                    var AllUsers = await _Context.Usuarios
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var rol in AllRoles)
                    {
                        roles.Add(new DetailRole()
                        {
                            nombre = rol.Nombre,
                            idRol = rol.IdRol,
                            cantOpciones = RelationatedRoles.Where(x => x.IdRol == rol.IdRol).ToList().Count(),
                            cantUsuarios = AllUsers.Where(x => x.IdRol == rol.IdRol).ToList().Count(),
                            status = rol.Status.Value
                        });
                    }
                    return roles;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Detalle de Roles");
            }
        }
        public async Task<List<Usuario>> GetAllUserAdminWithRol()
        {
            try
            {
                using (var _Context = new DBContext())
                {

                    return await _Context.Usuarios
                        .Include(x => x.IdPersonaNavigation)
                        .Include(x => x.IdRolNavigation)
                        .Where(x => x.IdRol != 3)
                        .ToListAsync();
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Usuarios con Roles");
            }
        }
        public async Task<List<RolOptionModules>> GetOptionsByRoles(int idRol)
        {
            try
            {
                List<RolOptionModules> rolesOptionModules = new();
                using (var _Context = new DBContext())
                {
                    List<RolOptions> optionsResponse = new();
                    var AllOptions = await _Context.ModuloInOpcions
                        .Include(x => x.IdOpcionNavigation)
                        .AsNoTracking()
                        .ToListAsync();

                    var rolOptions = await _Context.RolInOpcions
                        .Include(x => x.IdOpcionNavigation)
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var option in AllOptions)
                    {
                        if (rolesOptionModules.Count == 0 || rolesOptionModules.Last().idModulo != option.IdModulo)
                        {
                            rolesOptionModules.Add(new RolOptionModules()
                            {
                                idModulo = option.IdModulo,
                                titulo = option.IdOpcionNavigation.Titulo,
                                options = new List<RolOptions>()
                            });
                        }
                        rolesOptionModules.Last().options.Add(new RolOptions()
                        {
                            opcion = option.IdOpcionNavigation.Titulo,
                            idOption = option.IdOpcionNavigation.IdOpcion,
                            isActivate = rolOptions.Where(x => (x.IdOpcion == option.IdOpcion && x.IdRol == idRol))
                            .ToList().Count() > 0
                        });
                    }

                    
                    return rolesOptionModules;
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Opciones por rol");
            }
        }
        public async Task<List<Rol>> GetAllRoles()
        {
            try
            {
                return await Context.Rols
                    .Where(x => x.IdRol != 3 && x.IdRol != 1)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Roles detalles");
            }
        }
        public async Task<Boolean> DisabledRol(int idRol)
        {
            try
            {

                var rol = await Context.Rols
                    .Where(x => x.IdRol == idRol && x.IdRol != 3)
                    .FirstOrDefaultAsync();

                rol.Status = 0;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Desactivar Rol");
            }
        }
        public async Task<Boolean> EnableRol(int idRol)
        {
            try
            {

                var rol = await Context.Rols
                    .Where(x => x.IdRol == idRol && x.IdRol != 3)
                    .FirstOrDefaultAsync();

                rol.Status = 1;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Activar Rol");
            }
        }
        public async Task<Boolean> DisabledUser(int idUser)
        {
            try
            {

                var user = await Context.Usuarios
                    .Where(x => x.IdUsuario == idUser)
                    .FirstOrDefaultAsync();

                user.Status = 0;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Desactivar Rol");
            }
        }
        public async Task<Boolean> EnableUser(int idUser)
        {
            try
            {

                var user = await Context.Usuarios
                    .Where(x => x.IdUsuario == idUser)
                    .FirstOrDefaultAsync();

                user.Status = 1;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Activar Rol");
            }
        }
        public async Task<List<RolInOpcion>> UpdateRolPermissions(int idRol, List<RolOptionModules> modules)
        {
            try
            {
                var rolInOptions = await Context.RolInOpcions.
                    AsNoTracking()
                    .Where(x => x.IdRol == idRol)
                    .ToListAsync();

                foreach (var module in modules)
                {
                    foreach (var option in module.options)
                    {
                        if(
                            rolInOptions.Where(rolInOption =>
                                (rolInOption.IdRol == idRol && rolInOption.IdOpcion == option.idOption)
                            ).FirstOrDefault() == null
                        )
                        {
                            if (option.isActivate == false) continue;
                            await Context.RolInOpcions.AddAsync(new RolInOpcion()
                            {
                                IdOpcion = option.idOption,
                                IdRol = idRol
                            });
                        }
                        else
                        {
                            if (option.isActivate == true) continue;
                            Context.RolInOpcions.Remove(new RolInOpcion()
                            {
                                IdOpcion = option.idOption,
                                IdRol = idRol
                            });
                            
                        }
                        await Context.SaveChangesAsync();
                    }
                }

                return rolInOptions;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Administrar Roles");
            }
        }
        public async Task<Usuario> UpdateUserAdminByRol(Usuario user)
        {
            try
            {
                var specificUser = await Context.Usuarios.
                    Where(x => x.IdUsuario == user.IdUsuario)
                    .FirstOrDefaultAsync();

                specificUser.IdRol = user.IdRol;
                specificUser.Username = user.Username;
                specificUser.Password = user.Password;

                await Context.SaveChangesAsync();
                return specificUser;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Actualizar usuario");
            }
        }

    }
}

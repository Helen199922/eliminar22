using AutoMapper;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Roles.DTOs;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.Roles.Repository.IRepository;
using CarniceriaFinal.Roles.Services.IServices;
using CarniceriaFinal.Security.IRepository;
using CarniceriaFinal.Security.Services.IServices;
using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Roles.Services
{
    
    public class RolService : IRolService
    {
        private readonly IRolRepository IRolRepository;
        private readonly IUserService IUserService;
        private readonly IUserRepository userRepository;
        private readonly IMapper IMapper;
        public RolService(IRolRepository IRolRepository, IMapper IMapper, IUserService IUserService, IUserRepository IUserRepository)
        {
            this.IRolRepository = IRolRepository;
            this.IMapper = IMapper;
            this.IUserService = IUserService;
            this.userRepository = IUserRepository;
        }
        
        public async Task<List<DetailRole>> GetAllDetailRoles()
        {
            try
            {
                
                var response = await this.IRolRepository.GetAllDetailRoles();


                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de roles.");
            }
        }
        public async Task<List<RolOptionModules>> GetOptionsByRoles(int idRol)
        {
            try
            {

                var response = await this.IRolRepository.GetOptionsByRoles(idRol);


                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener las opciones por roles.");
            }
        }

        public async Task<RolEntity> GetRolByIdUser(int idUser)
        {
            try
            {
                var response = await IRolRepository.GetRolByIdUser(idUser);
                if (response == null)
                {
                    throw RSException.NoData("No hemos encontrado información del rol especificado");
                }
                return IMapper.Map<RolEntity>(response);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el usuario especificado.");
            }
        }
        public async Task<List<RolEntity>> GetAllRoles()
        {
            try
            {
                var response = await IRolRepository.GetAllRoles();
                if (response == null)
                {
                    throw RSException.NoData("No hemos encontrado información de los roles");
                }
                return IMapper.Map<List<RolEntity>>(response);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la información de los roles.");
            }
        }
        public async Task<Boolean> DisabledRol(int idRol)
        {
            try
            {
                var response = await this.IRolRepository.DisabledRol(idRol);
                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al desactivar el rol.");
            }
        }
        public async Task<Boolean> EnableRol(int idRol)
        {
            try
            {
                var response = await this.IRolRepository.EnableRol(idRol);
                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al activar el rol.");
            }
        }
        public async Task<List<RolOptionModules>> UpdateRolPermissions(int idRol, List<RolOptionModules> options)
        {
            try
            {
                var response = await this.IRolRepository.UpdateRolPermissions(idRol, options);

                return await this.GetOptionsByRoles(idRol);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al administrar los roles.");
            }
        }
        public async Task<UserEntity> CreateUserAdminByRol(DetailUserEntity user)
        {
            try
            {
                var response = await this.IUserService.CreateUserByRol(user);

                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al registrar el usuario de administración.");
            }
        }
        public async Task<DetailUserEntity> UpdateUserAdminByRol(DetailUserEntity user)
        {
            try
            {
                DetailUserEntity detailUser = new();

                if (user.usuario.idUsuario == null) throw RSException.BadRequest("No detectamos el usuario. Por favor, intente otra vez.");
                if (user.usuario.idRol == null) throw RSException.BadRequest("Por favor, ingrese el rol del usuario de administración.");
                var username = await userRepository.GetUserByUserName(user.usuario.username);
                if (username != null)
                {
                    var idUser = user.usuario.idUsuario != null ? user.usuario.idUsuario : 0;
                    var userSearch = await this.userRepository.GetUserByUserNameAndIdUser(user.usuario.username, idUser);
                    if (userSearch == null)
                        throw RSException.Unauthorized("Por favor, el usuario que intenta ingresar ya existe. Por favor, escoja un nuevo Username.");
                }

                var userAdminResponse = await this.IRolRepository.UpdateUserAdminByRol(IMapper.Map<Usuario>(user.usuario));
                detailUser.usuario = IMapper.Map<UserEntity>(userAdminResponse);
                detailUser.persona = await this.IUserService.ManagementPerson(user.persona);

                return detailUser;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar la información del usuario administrador.");
            }
        }
        public async Task<List<DetailUserEntity>> GetAllUsersAdmin()
        {
            try
            {
                //public PersonEntity persona { get; set; }
                //public UserEntity usuario { get; set; }

                List<DetailUserEntity> detailUser = new();

                List<Usuario> users = await this.IRolRepository.GetAllUserAdminWithRol();
                foreach (var user in users)
                {
                    var userValue = IMapper.Map<UserEntity>(user);
                    userValue.rolName = user.IdRolNavigation.Nombre;

                    detailUser.Add(new DetailUserEntity(){
                        persona = IMapper.Map<PersonEntity>(user.IdPersonaNavigation),
                        usuario = userValue
                    });
                }

                return detailUser;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar la información del usuario administrador.");
            }
        }
        public async Task<Boolean> DisabledUser(int idUser)
        {
            try
            {
                var response = await this.IRolRepository.DisabledUser(idUser);
                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al desactivar el usuario.");
            }
        }
        public async Task<Boolean> EnableUser(int idUser)
        {
            try
            {
                var response = await this.IRolRepository.EnableUser(idUser);
                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al activar el usuario.");
            }
        }
    }
}

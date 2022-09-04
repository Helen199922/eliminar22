using AutoMapper;
using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Repository.IRepository;
using CarniceriaFinal.Autenticacion.Services.IServices;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Security.DTOs;
using CarniceriaFinal.Security.IRepository;
using CarniceriaFinal.Security.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository ILoginRepository;
        private readonly IJwtUtils IJwtUtils;
        private readonly IMapper IMapper;
        public LoginService(ILoginRepository ILoginRepository, IMapper IMapper,
            IJwtUtils IJwtUtils)
        {
            this.ILoginRepository = ILoginRepository;
            this.IMapper = IMapper;
            this.IJwtUtils = IJwtUtils;
        }
        public async Task<UserAuthResponseEntity> Login(UserAuthRequestEntity user)
        {
            try
            {
                var responseRepo = await ILoginRepository.GetUserByUserName(user.username, user.password);
                var response = IMapper.Map<UserEntity>(responseRepo);

                if (response == null)
                    throw RSException.Unauthorized("Las claves de acceso son incorrectas");
                
                if (response.status == 0 || responseRepo.IdRolNavigation.Status == 0)
                    throw RSException.Unauthorized("El usuario está inhabilitado. Por favor, comuníquese con un administrador del sistema.");
                

                UserAuthResponseEntity userReponse = new();
                userReponse.idRol = response.idRol.Value;
                userReponse.idUser = response.idUsuario.Value;
                userReponse.username = user.username;
                if (userReponse.idRol != 3)
                {
                    userReponse.isAdminUser = true;
                    userReponse.menu = await this.Menu(userReponse.idRol);
                }
                else
                {
                    userReponse.isAdminUser = false;
                    userReponse.menu = new();
                }

                UserTokenEntity userEntity = new(){
                    idRol = response.idRol.Value,
                    idUser = response.idUsuario.Value
                };
                userReponse.token = IJwtUtils.generateJwtToken(userEntity);

                return userReponse;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al inciar sesión.");
            }
        }
        public async Task<List<MenuEntity>> Menu(int idRol)
        {
            try
            {
                var list = await ILoginRepository.GetMenuByIdRol(idRol);
                List<MenuEntity> menu = new();
                List<Item> items = null;

                foreach (var item in list)
                {
                    if (menu.Count == 0 || (menu.Count > 0 && menu.Last().id != item.IdModulo))
                        items = new();

                    items.Add(new() {
                        id = item.IdOpcionNavigation.IdOpcion,
                        icon = item.IdOpcionNavigation.Icon,
                        option = item.IdOpcionNavigation.Titulo,
                        route = item.IdOpcionNavigation.Route
                    });
                    if (menu.Count > 0 && menu.Last().id == item.IdModulo) continue;

                    menu.Add(new() {
                        icon = item.IdModuloNavigation.Icon,
                        id = item.IdModuloNavigation.IdModulo,
                        module = item.IdModuloNavigation.Titulo,
                        route = item.IdModuloNavigation.Route,
                        items = items
                    });
                }

                

                return menu;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el menú.");
            }
        }
    }
}

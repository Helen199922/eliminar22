using AutoMapper;
using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Services.IServices;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.IRepository;
using CarniceriaFinal.Security.Services.IServices;
using CarniceriaFinal.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.User.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository IUserRepository;
        private readonly IPersonRepository IPersonRepository;
        private readonly ILoginService ILoginService;
        private readonly IMapper IMapper;
        public UserService(IUserRepository IUserRepository, IPersonRepository IPersonRepository, IMapper IMapper, ILoginService ILoginService)
        {
            this.IUserRepository = IUserRepository;
            this.IPersonRepository = IPersonRepository;
            this.ILoginService = ILoginService;
            this.IMapper = IMapper;
        }
        public async Task<UserAuthResponseEntity> CreateUser(DetailUserEntity user)
        {
            try
            {
                user.usuario.idRol = 3;
                var username = await IUserRepository.GetUserByUserName(user.usuario.username);
                if (username != null) throw RSException.Unauthorized("Por favor, el usuario que intenta ingresar ya existe. Por favor, escoja un nuevo Username.");

                //Si ya esta creado el usuario con estas credenciales, se debe enviar un mensaje de error para que inicie sesión
                if ( await IUserRepository
                    .GetUserByIdIndentificationPerson(user.persona.cedula) != null
                   )
                {
                    throw RSException.Unauthorized("Ya existe un usuario registrado con estas credenciales. Por favor, inicie sesión.");
                }
                //Gestionar la creación o actualización de la persona
                PersonEntity person = await this.ManagementPerson(user.persona);

                //Creamos el usuario
                user.usuario.idPersona = person.idPersona.Value;
                await IUserRepository.CreateUser(IMapper.Map<Usuario>(user.usuario));

                return await ILoginService.Login(new() { 
                    password = user.usuario.password,
                    username = user.usuario.username
                });
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al registrar al usuario. Por favor, intente más tarde.");
            }
        }

        public async Task<UserEntity> CreateUserByRol(DetailUserEntity user)
        {
            try
            {
                if (user.usuario.idRol == null) throw RSException.BadRequest("Por favor, ingrese el rol del usuario de administración.");
                var username = await IUserRepository.GetUserByUserName(user.usuario.username);
                if(username != null) throw RSException.Unauthorized("Por favor, el usuario que intenta ingresar ya existe. Por favor, escoja un nuevo Username.");
                //Si ya esta creado el usuario con estas credenciales, se debe enviar un mensaje de error para que inicie sesión
                if (await IUserRepository
                    .GetUserRolByIdIndentificationPerson(user.persona.cedula) != null
                   )
                {
                    throw RSException.Unauthorized("Ya existe un usuario de administración registrado con estas credenciales. Por favor, inicie sesión o actualice la información.");
                }
                //Gestionar la creación o actualización de la persona
                PersonEntity person = await this.ManagementPerson(user.persona);

                //Creamos el usuario
                user.usuario.idPersona = person.idPersona.Value;
                var userCreated = await IUserRepository.CreateUser(IMapper.Map<Usuario>(user.usuario));

                return IMapper.Map<UserEntity>(userCreated);
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
        public async Task<PersonEntity> ManagementPerson(PersonEntity person)
        {
            try
            {
                PersonEntity response = IMapper.Map<PersonEntity>(
                    await IPersonRepository.GetPersonByIdentification(person.cedula)
                );


                if (response != null)
                {
                    //Actualizar
                    person.idPersona = response.idPersona;
                    return IMapper.Map<PersonEntity>(
                        await IPersonRepository
                        .UpdatePerson(IMapper.Map<Persona>(person))
                        );
                }
                else
                {
                    //Crear
                    var personn = IMapper.Map<Persona>(person);
                    return IMapper.Map<PersonEntity>(
                        await IPersonRepository
                            .CreatePerson(personn)
                        );
                }
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido al registrar el usuario y/o rol.");
            }
        }
        public async Task<PersonEntity> GetPersonByUserName(string username)
        {
            try
            {
                var reponseRepo = await IUserRepository.GetPersonByUserName(username);
                if (reponseRepo == null)
                    throw RSException.Unauthorized("No tiene autorización para hacer esta consulta");
                PersonEntity response = IMapper.Map<PersonEntity>(reponseRepo.IdPersonaNavigation);
                
                return response;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los usuarios.");
            }
        }
        public async Task<List<DetailUserEntity>> GetAllUsers()
        {
            try
            {
                var response = await IUserRepository.GetAllUser();
                if(response == null || response.Count == 0)
                {
                    return new List<DetailUserEntity>();
                }
                List<DetailUserEntity> details = new();
                DetailUserEntity detail = null;
                foreach (var item in response)
                {
                    detail = new();
                    detail.usuario = IMapper.Map<UserEntity>(item);
                    detail.persona = IMapper.Map<PersonEntity>(item.IdPersonaNavigation);
                    details.Add(detail);
                }
                return details;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los usuarios.");
            }
        }
        public async Task<List<DetailUserEntity>> GetProfileInfo()
        {
            try
            {
                var response = await IUserRepository.GetAllUser();
                if (response == null || response.Count == 0)
                {
                    return new List<DetailUserEntity>();
                }
                List<DetailUserEntity> details = new();
                DetailUserEntity detail = null;
                foreach (var item in response)
                {
                    detail = new();
                    detail.usuario = IMapper.Map<UserEntity>(item);
                    detail.persona = IMapper.Map<PersonEntity>(item.IdPersonaNavigation);
                    details.Add(detail);
                }
                return details;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los usuarios.");
            }
        }

    }
}

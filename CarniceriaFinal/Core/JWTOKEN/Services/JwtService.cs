using AutoMapper;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.JWTOKEN.Repository.IRepository;
using CarniceriaFinal.Core.JWTOKEN.Services.IServices;
using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.JWTOKEN.Services
{
    public class JwtService : IJwtService
    {
        private readonly IJWTRepository JWTRepository;
        private readonly IMapper IMapper;
        public JwtService(IJWTRepository IJWTRepository, IMapper IMapper)
        {
            this.JWTRepository = IJWTRepository;
            this.IMapper = IMapper;
        }

        public async Task<UserEntity> GetUserById(int idUser)
        {
            try
            {
                return IMapper.Map<UserEntity>( await JWTRepository.GetUserById(idUser));

                
            }
            catch (Exception err)
            {
                return null;
            }
        }

        public async Task<Boolean> IsPublicEndPoint(string endPoint, string method)
        {
            try
            {
                return await JWTRepository.IsPublicEndPoint(endPoint, method);


            }
            catch (Exception err)
            {
                return false;
            }
        }
        public async Task<Boolean> IsOnlyForUSer(string endPoint, string method)
        {
            try
            {
                return await JWTRepository.IsOnlyForUSer(endPoint, method);


            }
            catch (Exception err)
            {
                return false;
            }
        }

        public async Task<Boolean> FindOptionByIdRolAndMethodAndEndPoint(int idRol, string endPoint, string method)
        {
            try
            {
                var option = JWTRepository.FindOptionByIdRolAndMethodAndEndPoint(idRol, endPoint, method);

                if (option == null) return false;

                return true;


            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}

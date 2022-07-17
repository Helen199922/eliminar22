using CarniceriaFinal.Autenticacion.DTOs;
using CarniceriaFinal.Autenticacion.Services.IServices;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Security.Services.IServices;
using CarniceriaFinal.User.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Autenticacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        public readonly ILoginService ILoginService;
        public readonly IUserService IUserService;
        public AutenticacionController(ILoginService ILoginService, IUserService IUserService)
        {
            this.ILoginService = ILoginService;
            this.IUserService = IUserService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthRequestEntity user)
        {
            RSEntity<UserAuthResponseEntity> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ILoginService.Login(user)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] DetailUserEntity user)
        {
            RSEntity<UserAuthResponseEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IUserService.CreateUser(user)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPost("by-rol/register")]
        public async Task<IActionResult> CreateUserByRol([FromBody] DetailUserEntity user)
        {
            RSEntity<UserEntity> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await IUserService.CreateUserByRol(user)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

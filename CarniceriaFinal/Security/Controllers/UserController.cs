using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.Roles.Services.IServices;
using CarniceriaFinal.Security.Services.IServices;
using CarniceriaFinal.User.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService IUserService;
        public UserController(IUserService IUserService)
        {
            this.IUserService = IUserService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> Login(string username)
        {
            RSEntity<PersonEntity> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await IUserService.GetPersonByUserName(username)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

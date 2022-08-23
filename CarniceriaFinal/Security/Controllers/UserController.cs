using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.Roles.Services.IServices;
using CarniceriaFinal.Security.DTOs;
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
        [Authorize]
        [HttpGet("profile/{idUser}")]
        public async Task<IActionResult> GetProfileInfo(int idUser)
        {
            RSEntity<ProfileUserEntity> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await IUserService.GetProfileInfo(idUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPut("profile/{idUser}")]
        public async Task<IActionResult> UpdateProfileInfo(int idUser, [FromBody] ProfileUserEntity profile)
        {
            RSEntity<ProfileUserEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IUserService.UpdateProfileInfo(profile, idUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPut("profile/status-received-email/{idUser}")]
        public async Task<IActionResult> UpdateStatusReceivedEmailByIdUser([FromBody] ChangeReceivedEmailUserEntity profile, int idUser)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IUserService.UpdateStatusReceivedEmailByIdUser(idUser, profile.status)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

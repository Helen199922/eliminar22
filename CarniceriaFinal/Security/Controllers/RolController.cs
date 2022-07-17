using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Roles.DTOs;
using CarniceriaFinal.Roles.Repository;
using CarniceriaFinal.Roles.Services.IServices;
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
    public class RolController : ControllerBase
    {
        private readonly IRolService IRolService;
        public RolController(IRolService IRolService)
        {
            this.IRolService = IRolService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDetailRoles()
        {
            RSEntity<List<DetailRole>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.GetAllDetailRoles()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            RSEntity<List<RolEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.GetAllRoles()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("{idRol}")]
        public async Task<IActionResult> GetOptionsByRoles(int idRol)
        {
            RSEntity<List<RolOptionModules>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.GetOptionsByRoles(idRol)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("users-admin")]
        public async Task<IActionResult> GetAllUserAdmin()
        {
            RSEntity<List<DetailUserEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.GetAllUsersAdmin()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpDelete("user-admin/{idUser}")]
        public async Task<IActionResult> DisableUser(int idUser)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.DisabledUser(idUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPatch("user-admin/{idUser}")]
        public async Task<IActionResult> EnableUser(int idUser)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.EnableUser(idUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpDelete("{idRol}")]
        public async Task<IActionResult> DisabledRol(int idRol)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.DisabledRol(idRol)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPatch("{idRol}")]
        public async Task<IActionResult> EnableRol(int idRol)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.EnableRol(idRol)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPut("{idRol}")]
        public async Task<IActionResult> UpdateRolPermissions(int idRol, [FromBody] List<RolOptionModules> options)
        {
            RSEntity<List<RolOptionModules>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.UpdateRolPermissions(idRol, options)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAdminByRol([FromBody] DetailUserEntity user)
        {
            RSEntity<UserEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.CreateUserAdminByRol(user)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUserAdminByRol([FromBody] DetailUserEntity user)
        {
            RSEntity<DetailUserEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRolService.UpdateUserAdminByRol(user)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

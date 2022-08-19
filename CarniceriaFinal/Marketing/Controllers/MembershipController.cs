﻿using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarniceriaFinal.Marketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        public readonly IMembershipService IMembershipService;
        public MembershipController(IMembershipService IMembershipService)
        {
            this.IMembershipService = IMembershipService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMembershipHome()
        {
            RSEntity<MemberShipCommEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IMembershipService.GetMembershipHome()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
       [HttpGet("admin-members")]
        public async Task<IActionResult> AdministrationMembershipTimes()
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IMembershipService.AdministrationMembershipTimes()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}
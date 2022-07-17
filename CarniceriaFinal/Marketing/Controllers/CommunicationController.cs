using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        public readonly ICommunicationService ICommunicationService;
        public CommunicationController(ICommunicationService ICommunicationService)
        {
            this.ICommunicationService = ICommunicationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            RSEntity<List<CommunicationEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ICommunicationService.GetAll()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Security.DTOs;
using CarniceriaFinal.Security.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarniceriaFinal.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaServices ICaptchaServices;
        public CaptchaController(ICaptchaServices ICaptchaServices)
        {
            this.ICaptchaServices = ICaptchaServices;
        }
        [HttpPost]
        public async Task<IActionResult> IsCaptchaValid([FromBody] IsValidCaptchaRequest data)
        {
            RSEntity<Boolean> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ICaptchaServices.IsCaptchaValid(data.Token)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

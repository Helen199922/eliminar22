using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Core.Email.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.Email.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        public EmailController(IEmailService mailService)
        {
            this.emailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] EmailRequest request)
        {
            try
            {
                await emailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }

        }
    }
}

using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarniceriaFinal.Marketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionalEmailController : ControllerBase
    {
        public readonly IPromotionalEmailService IPromotionalEmailServi;
        public PromotionalEmailController(IPromotionalEmailService IPromotionalEmailService)
        {
            IPromotionalEmailServi = IPromotionalEmailService;
        }
        [HttpGet("{idPromotion}")]
        public async Task<IActionResult> GetEmailByidPromotion(int idPromotion)
        {
            RSEntity<EmailResponseEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.GetEmailByidPromotion(idPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmailByIdPromotion([FromBody] EmailCreateEntity emailPromotion)
        {
            RSEntity<EmailSenderStatusEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.CreateEmailByIdPromotion(emailPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateEmailByidEmail([FromBody] EmailCreateEntity emailPromotion)
        {
            RSEntity<EmailResponseEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.UpdateEmailByidEmail(emailPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("send-email/{idPromotion}/{idCorreoPromotion}")]
        public async Task<IActionResult> SendPromotionalEmail(int idPromotion, int idCorreoPromotion)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.SendPromotionalEmail(idPromotion, idCorreoPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("retry-send-email/{idPromotion}/{idCorreoPromotion}")]
        public async Task<IActionResult> RetrySenderEmailByIdPromotion(int idPromotion, int idCorreoPromotion)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.RetrySenderEmailByIdPromotion(idPromotion, idCorreoPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("cancel-send-email/{idPromotion}")]
        public async Task<IActionResult> CancelSenderEmailByIdPromotion(int idPromotion)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionalEmailServi.CancelSenderEmailByIdPromotion(idPromotion)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

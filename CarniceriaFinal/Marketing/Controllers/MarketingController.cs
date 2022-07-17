using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Interfaces.IService;
using CarniceriaFinal.ModelsEF;
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
    public class MarketingController : ControllerBase
    {
        public readonly IPromotionService IPromotionService;
        public MarketingController(IPromotionService IPromotionService)
        {
            this.IPromotionService = IPromotionService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] Promocion promo)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await IPromotionService.CreatePromotion(promo);
                return Ok(rsEntity.Send("Promoción creada correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            RSEntity<List<Promocion>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionService.GetAll()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

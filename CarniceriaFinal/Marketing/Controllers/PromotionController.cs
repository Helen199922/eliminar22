using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
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
    public class PromotionController : ControllerBase
    {
        public readonly IPromotionService IPromotionService;
        public PromotionController(IPromotionService IPromotionService)
        {
            this.IPromotionService = IPromotionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            RSEntity<List<PromotionEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionService.GetAll()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("products-to-promo/{idPromotion}")]
        public async Task<IActionResult> GetAllProductsByIdPromotion(int? idPromotion)
        {
            RSEntity<List<PromotionProductEntity>> rsEntity = new();
            try
            {
                var result = await IPromotionService.GetAllProductsByIdPromotion(idPromotion);
                return Ok(rsEntity.Send(result));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionEntity promo)
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
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdatePromotion([FromBody] PromotionEntity promo)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await IPromotionService.UpdatePromotion(promo);
                return Ok(rsEntity.Send("Promoción actualizada correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPatch("{status}/{idPromotion}")]
        public async Task<IActionResult> StatusPromotion(int status, int idPromotion)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await IPromotionService.StatusPromotion(status, idPromotion);
                return Ok(rsEntity.Send("Estatus de promoción actualizada correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost("isAvailability-promotion")]
        public async Task<IActionResult> isAvailabilityToCreatePromotion([FromBody] IsAvailabilityCreatePromoEntity data)
        {
            RSEntity<String> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionService.isAvailabilityToCreatePromotion(data)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("all-dscts-promotions")]
        public async Task<IActionResult> GetAllDsctPromotion()
        {
            RSEntity<List<PorcentajeDscto>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IPromotionService.GetAllDsctPromotion()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

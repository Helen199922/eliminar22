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
    public class RecommendationController : ControllerBase
    {
        public readonly IRecommendationService IRecommendationService;
        public RecommendationController(IRecommendationService IRecommendationService)
        {
            this.IRecommendationService = IRecommendationService;
        }
        [HttpGet("momento-degustacion")]
        public async Task<IActionResult> GetAllTimesToEat()
        {
            RSEntity<List<TimesToEatEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetAllTimesToEat()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("user-momento-degustacion")]
        public async Task<IActionResult> GetAllTimesToEatGeneralUser()
        {
            RSEntity<List<TimesToEatEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetAllTimesToEatGeneralUser()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("cambiar-estado-momento-degustacion")]
        public async Task<IActionResult> ChangeStatusTimesToEat([FromBody] AdmStatusRecommendation data)
        {
            RSEntity<TimesToEatEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.ChangeStatusTimesToEat(data.id, data.status)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("modo-preparacion")]
        public async Task<IActionResult> GetAllPreparationWays()
        {
            RSEntity<List<PreparationWay>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetAllPreparationWays()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("user-modo-preparacion/{idTimeToEat}")]
        public async Task<IActionResult> GetAllPreparationWaysGeneralUser(int idTimeToEat)
        {
            RSEntity<List<PreparationWay>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetAllPreparationWaysGeneralUser(idTimeToEat)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("cambiar-estado-modo-preparacion")]
        public async Task<IActionResult> ChangeStatusPreparationWays([FromBody] AdmStatusRecommendation data)
        {
            RSEntity<PreparationWay> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.ChangeStatusPreparationWays(data.id, data.status)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("detalle-momento-degustacion/{idTimeToEat}")]
        public async Task<IActionResult> GetTimeToEatDetail(int idTimeToEat)
        {
            RSEntity<CreateTimesToEatWay> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetTimeToEatDetail(idTimeToEat)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("detalle-modo-preparacion/{idPreparationWay}")]
        public async Task<IActionResult> GetPreparationWayDetail(int idPreparationWay)
        {
            RSEntity<CreatePreparationWay> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetPreparationWayDetail(idPreparationWay)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("crear-modo-preparacion")]
        public async Task<IActionResult> CreatePreparationWay([FromBody] CreatePreparationWay data)
        {
            RSEntity<PreparationWay> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.CreatePreparationWay(data)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("crear-momento-degustacion")]
        public async Task<IActionResult> CreateTimeToEat([FromBody] CreateTimesToEatWay data)
        {
            RSEntity<TimesToEatEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.CreateTimeToEat(data)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("actualizar-modo-preparacion")]
        public async Task<IActionResult> UpdatePreparationWay([FromBody] CreatePreparationWay data)
        {
            RSEntity<PreparationWay> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.UpdatePreparationWay(data)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("actualizar-momento-degustacion")]
        public async Task<IActionResult> UpdateTimeToEat([FromBody] CreateTimesToEatWay data)
        {
            RSEntity<TimesToEatEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.UpdateTimeToEat(data)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpGet("obtener-eventos-especiales")]
        public async Task<IActionResult> GetAllSpecialEvent()
        {
            RSEntity<List<SpecialEventEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetAllSpecialEvent()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("crear-eventos-especiales")]
        public async Task<IActionResult> CreateSpecialEvent([FromBody] SpecialEventEntity specialEvent)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.CreateSpecialEvent(specialEvent)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("actualizar-eventos-especiales")]
        public async Task<IActionResult> UpdateSpecialEvent([FromBody] SpecialEventEntity specialEvent)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.UpdateSpecialEvent(specialEvent)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("desactivar-eventos-especiales/{idSpecialEvent}")]
        public async Task<IActionResult> DisableSpecialEvent(int idSpecialEvent)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.DisableSpecialEvent(idSpecialEvent)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPut("activar-eventos-especiales/{idSpecialEvent}")]
        public async Task<IActionResult> EnableSpecialEvent(int idSpecialEvent)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.EnableSpecialEvent(idSpecialEvent)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("obtener-eventos-especiales-byid/{idSpecialEvent}")]
        public async Task<IActionResult> GetSpecialEventByIdEvent(int idSpecialEvent)
        {
            RSEntity<SpecialEventEntity> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IRecommendationService.GetSpecialEventByIdEvent(idSpecialEvent)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

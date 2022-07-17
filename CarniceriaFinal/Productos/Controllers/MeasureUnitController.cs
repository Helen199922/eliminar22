using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasureUnitController : ControllerBase
    {
        public readonly IMeasureUnitService measureUnitService;
        public MeasureUnitController(IMeasureUnitService measureUnitService)
        {
            this.measureUnitService = measureUnitService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateMeasureUnit([FromBody] MeasureUnitEntity unidad)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                await measureUnitService.CreateMeasureUnit(unidad.unidad);
                return Ok(rsEntity.Send("correctamente"));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeasureUnits()
        {
            RSEntity<List<MeasureUnitEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await measureUnitService.GetAllMeasureUnit()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

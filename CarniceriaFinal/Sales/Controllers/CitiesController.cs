using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        public readonly ICitiesServices ICitiesServices;
        public CitiesController(ICitiesServices ICitiesServices)
        {
            this.ICitiesServices = ICitiesServices;
        }

        [HttpGet("costo-envio")]
        public async Task<IActionResult> GetAllSalesStatus()
        {
            RSEntity<List<ProvincesCityCost>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ICitiesServices.GetCitiesCost()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [HttpPut("costo-envio")]
        public async Task<IActionResult> UpdateCitiesCost(List<CityCost> cities)
        {
            RSEntity<List<ProvincesCityCost>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await ICitiesServices.UpdateCitiesCost(cities)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

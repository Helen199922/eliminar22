using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarniceriaFinal.Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesAdmController : ControllerBase
    {
        public readonly ISalesAdmServices ISalesAdmServices;
        public SalesAdmController(ISalesAdmServices ISalesAdmServices)
        {
            this.ISalesAdmServices = ISalesAdmServices;
        }
        [HttpPost("sales-by-status/{idStatus}")]
        public async Task<IActionResult> GetAllSales(int idStatus)
        {
            RSEntity<List<SalesAdmEntity>> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesAdmServices.GetAllSales(idStatus)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

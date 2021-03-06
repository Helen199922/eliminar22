using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.IServices;
using CarniceriaFinal.Sales.Models;
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
    public class SalesController : ControllerBase
    {
        public readonly ISalesService ISalesService;
        public SalesController(ISalesService ISalesService)
        {
            this.ISalesService = ISalesService;
        }
        [HttpPost("no-user")]
        public async Task<IActionResult> CreateSaleNoUser([FromBody] SaleNoUserRequestEntity saleNoUser)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                
                return Ok(rsEntity.Send(await ISalesService.CreateSaleNoUser(saleNoUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("user")]
        public async Task<IActionResult> CreateSaleUser([FromBody] SaleNoUserRequestEntity saleNoUser)
        {
            RSEntity<string> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.CreateSaleUser(saleNoUser)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPatch("status/{idSale}/status/{idStatus}")]
        public async Task<IActionResult> UpdateStatus(int idSale, int idStatus)
        {
            RSEntity<string> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.UpdateStateSale(idStatus, idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpDelete("{idSale}")]
        public async Task<IActionResult> DeleteSale(int idSale)
        {
            RSEntity<string> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.UpdateStateSale(4, idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("type-status")]
        public async Task<IActionResult> GetAllSalesStatus()
        {
            RSEntity<List<SalesStatus>> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.GetAllSalesStatus()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            RSEntity<List<SalesCompleteEntity>> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.GetAllSales()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("{idClient}")]
        public async Task<IActionResult> GetAllSalesByIdClient(int idClient)
        {
            RSEntity<List<SalesCompleteEntity>> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesService.GetAllSalesByIdClient(idClient)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

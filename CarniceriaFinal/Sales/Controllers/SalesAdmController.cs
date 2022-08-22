﻿using CarniceriaFinal.Core;
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
        [HttpGet("sales-by-status/{idStatus}")]
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
        [HttpGet("detalle-venta/{idSale}")]
        public async Task<IActionResult> GetDetailByIdSale(int idSale)
        {
            RSEntity<ResumeSaleDetail> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesAdmServices.GetDetailByIdSale(idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("atender-venta")]
        public async Task<IActionResult> attendSale([FromBody] SaleAdmRequestIdSale sale)
        {
            RSEntity<string> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesAdmServices.attendSale(sale.idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("rechazar-venta")]
        public async Task<IActionResult> declineSale([FromBody] SaleAdmRequestIdSale sale)
        {
            RSEntity<string> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesAdmServices.declineSale(sale.idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

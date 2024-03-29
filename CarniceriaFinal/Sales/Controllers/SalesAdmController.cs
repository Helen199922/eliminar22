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
        public readonly IHttpContextAccessor httpContextAccessor;
        public SalesAdmController(ISalesAdmServices ISalesAdmServices, IHttpContextAccessor httpContextAccessor)
        {
            this.ISalesAdmServices = ISalesAdmServices;
            this.httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost("atender-venta")]
        public async Task<IActionResult> AttendSale([FromBody] SaleAdmRequestIdSale sale)
        {
            RSEntity<string> rsEntity = new();
            try
            {
                try
                {
                    var idUser = (int)(httpContextAccessor.HttpContext.Items["idUser"]);
                    return Ok(rsEntity.Send(await ISalesAdmServices.attendSale(sale.idSale, idUser)));
                }
                catch (Exception err)
                {
                    return StatusCode(400, rsEntity.Fail("Hemos detectado una inconsistencía en el registro de la venta. Vuelva a intentar más tarde."));
                }
                
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost("rechazar-venta")]
        public async Task<IActionResult> DeclineSale([FromBody] SaleAdmRequestIdSale sale)
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


        [HttpGet("sales-detail-to-sale/{idSale}")]
        public async Task<IActionResult> GetSaleDetailById(int idSale)
        {
            RSEntity<SaleAdmRequestSaleDetail> rsEntity = new();
            try
            {

                return Ok(rsEntity.Send(await ISalesAdmServices.GetSaleDetailById(idSale)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

using CarniceriaFinal.Core;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarniceriaFinal.Reportes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly IReportServices IReportServices;
        public ReportController(IReportServices reportServi)
        {
            IReportServices = reportServi;
            this.IReportServices = reportServi;
        }
        [HttpPost("report-sale-by-dates")]
        public async Task<IActionResult> GetAllSalesToReportByDates([FromBody] ReportByDates data)
        {
            RSEntity<ReportResponse<List<FieldReportEntity>>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllSalesToReportByDates(data.timeStart, data.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("report-sale-by-dates-document")]
        public async Task<IActionResult> GetDetailSalesToReportByDates([FromBody] ReportByDates data)
        {
            RSEntity<DataSalesReportDetail> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetDetailSalesToReportByDates(data.timeStart, data.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpPost("report-sale-by-dates-and-category")]
        public async Task<IActionResult> GetAllProductsMostSalesByCategoryAndDates([FromBody] ReportByDatesAndCategory data)
        {
            RSEntity<ReportResponse<List<MiltiFieldReportEntity>>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllProductsMostSalesByCategoryAndDates(data.timeStart, data.timeEnd, data.idCategory)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [HttpGet("categories-for-report")]
        public async Task<IActionResult> GetListCategories()
        {
            RSEntity<List<CategoriesReports>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetListCategories()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
    }
}

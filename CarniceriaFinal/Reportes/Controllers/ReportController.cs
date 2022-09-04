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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost("report-products-top-by-dates-and-category")]
        public async Task<IActionResult> GetTopTenProductsMostSalesAndDates([FromBody] ReportByDates data)
        {
            RSEntity<ReportResponse<List<FieldReportAmountEntity>>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetTopTenProductsMostSalesAndDates(data.timeStart, data.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
        [Authorize]
        [HttpPost("report-sale-by-dates-and-category-document")]
        public async Task<IActionResult> GetAllProductsMostSalesByCategoryAndDatesDetail([FromBody] ReportByDatesAndCategory data)
        {
            RSEntity<List<ProductReportDetail>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllProductsMostSalesByCategoryAndDatesDetail(data.timeStart, data.timeEnd, data.idCategory)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
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
        [Authorize]
        [HttpPost("report-modules-by-dates")]
        public async Task<IActionResult> GetAllLogsModulesGraficsByDates([FromBody] ReportGraficsByDatesAndModules data)
        {
            RSEntity<List<MiltiFieldReportEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllLogsModulesGraficsByDates(data.idModules, data.timeStart, data.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("modules-for-report")]
        public async Task<IActionResult> GetAllListModules()
        {
            RSEntity<List<ModulesReports>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllListModules()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpPost("report-modules-by-dates-document")]
        public async Task<IActionResult> GetAllLogsModulesByDatesDetail([FromBody] ReportGraficsByDatesAndModules data)
        {
            RSEntity<ModulesReportDetail> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllLogsModulesByDatesDetail(data.idModules, data.timeStart, data.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("miembros-for-simple-report")]
        public async Task<IActionResult> GetSimpleMembershipReport()
        {
            RSEntity<List<FieldReportEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetSimpleMembershipReport()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        [Authorize]
        [HttpGet("miembros-for-detail-report")]
        public async Task<IActionResult> GetDetailMembershipReport()
        {
            RSEntity<List<MembershipUserDetailEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetDetailMembershipReport()));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPost("users-adm-report-sales")]
        public async Task<IActionResult> GetAllSalesToAdms([FromBody] ReportByDates dates)
        {
            RSEntity<List<FieldReportAmountEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetAllSalesToAdms(dates.timeStart, dates.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }

        [Authorize]
        [HttpPost("users-adm-detail-report-sales")]
        public async Task<IActionResult> GetDetailSalesToAdms([FromBody] ReportByDates dates)
        {
            RSEntity<List<SaleDetailAdmReportEntity>> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(await IReportServices.GetDetailSalesToAdms(dates.timeStart, dates.timeEnd)));
            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }
        }
        
    }
}

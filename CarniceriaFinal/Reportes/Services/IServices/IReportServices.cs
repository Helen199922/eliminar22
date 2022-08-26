using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;

namespace CarniceriaFinal.Reportes.Services.IServices
{
    public interface IReportServices
    {
        Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<DataSalesReportDetail> GetDetailSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<ReportResponse<List<MiltiFieldReportEntity>>> GetAllProductsMostSalesByCategoryAndDates(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<ProductReportDetail>> GetAllProductsMostSalesByCategoryAndDatesDetail(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<CategoriesReports>> GetListCategories();
        Task<List<MiltiFieldReportEntity>> GetAllLogsModulesGraficsByDates(List<int> idModules, DateTime timeStart, DateTime timeEnd);
        Task<List<ModulesReports>> GetAllListModules();
        Task<ModulesReportDetail> GetAllLogsModulesByDatesDetail(List<int> idModules, DateTime timeStart, DateTime timeEnd);
    }
}

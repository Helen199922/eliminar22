using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;

namespace CarniceriaFinal.Reportes.Repository.IRepository
{
    public interface IReportRepository
    {
        Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<List<Ventum>> GetDetailSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<ReportResponse<List<MiltiFieldReportEntity>>> GetAllProductsMostSalesByCategoryAndDates(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<ProductReportDetail>> GetAllProductsMostSalesByCategoryAndDatesDetail(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<CategoriesReports>> GetListCategories();
        Task<List<ModulesReports>> GetAllListModules();
        Task<List<MiltiFieldReportEntity>> GetAllLogsModulesGraficsByDates(List<int> idModules, DateTime timeStart, DateTime timeEnd);
        Task<ModulesReportDetail> GetAllLogsModulesByDatesDetail(List<int> idModules, DateTime timeStart, DateTime timeEnd);
        Task<ReportResponse<List<FieldReportAmountEntity>>> GetTopTenProductsMostSalesAndDates(DateTime timeStart, DateTime timeEnd);
        Task<List<FieldReportEntity>> GetSimpleMembershipReport();
        Task<List<MembershipUserDetailEntity>> GetDetailMembershipReport();
        Task<List<FieldReportAmountEntity>> GetAllSalesToAdms(DateTime timeStart, DateTime timeEnd);
        Task<List<SaleDetailAdmReportEntity>> GetDetailSalesToAdms(DateTime timeStart, DateTime timeEnd);
    }
}

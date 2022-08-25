using CarniceriaFinal.Reportes.DTOs;

namespace CarniceriaFinal.Reportes.Repository.IRepository
{
    public interface IReportRepository
    {
        Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<ReportResponse<List<MiltiFieldReportEntity>>> GetAllProductsMostSalesByCategoryAndDates(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<CategoriesReports>> GetListCategories();
    }
}

using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Repository.IRepository;
using CarniceriaFinal.Reportes.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Reportes.Services
{
    public class ReportServices : IReportServices
    {
        public readonly IReportRepository IReportRepository;
        public ReportServices(IReportRepository IReportRepository)
        {
            this.IReportRepository = IReportRepository;
        }

        //Reporte de ventas por fecha
        public async Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                return await IReportRepository.GetAllSalesToReportByDates(timeStart, timeEnd);
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de ventas por fecha");
            }
        }
        //Obtener los productois por categoria mas vendidos
        public async Task<ReportResponse<List<MiltiFieldReportEntity>>> GetAllProductsMostSalesByCategoryAndDates(DateTime timeStart, DateTime timeEnd, int idCategory)
        {
            try
            {
                return await IReportRepository.GetAllProductsMostSalesByCategoryAndDates(timeStart, timeEnd, idCategory);

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de productos");
            }
        }
        public async Task<List<CategoriesReports>> GetListCategories()
        {
            try
            {
                return await IReportRepository.GetListCategories();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de categorias");
            }
        }
    }
}

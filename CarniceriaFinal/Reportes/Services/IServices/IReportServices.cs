﻿using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;

namespace CarniceriaFinal.Reportes.Services.IServices
{
    public interface IReportServices
    {
        Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd);
        Task<ReportResponse<List<MiltiFieldReportEntity>>> GetAllProductsMostSalesByCategoryAndDates(DateTime timeStart, DateTime timeEnd, int idCategory);
        Task<List<CategoriesReports>> GetListCategories();
    }
}

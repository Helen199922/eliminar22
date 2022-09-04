using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Repository.IRepository;
using CarniceriaFinal.Reportes.Services.IServices;
using CarniceriaFinal.Sales.Models;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Reportes.Services
{
    public class ReportServices : IReportServices
    {
        public readonly IReportRepository IReportRepository;
        private readonly IMapper IMapper;
        public ReportServices(IReportRepository IReportRepository, IMapper IMapper)
        {
            this.IReportRepository = IReportRepository;
            this.IMapper = IMapper;
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
        public async Task<DataSalesReportDetail> GetDetailSalesToReportByDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                DataSalesReportDetail dataSalesReportDetail = new DataSalesReportDetail();
                var responseRepo = (await IReportRepository.GetDetailSalesToReportByDates(timeStart, timeEnd));


                List<SalesReportDetail> response = new();

                foreach (var x in responseRepo)
                {
                    var value = new SalesReportDetail()
                    {
                        cedula = x?.IdClienteNavigation?.IdPersonaNavigation?.Cedula != null ? x?.IdClienteNavigation?.IdPersonaNavigation?.Cedula : "",
                        ciudad = x?.IdCiudadNavigation?.Ciudad1 != null ? x?.IdCiudadNavigation?.Ciudad1 : "",
                        direccion = x?.Direccion != null ? x?.Direccion : "",
                        costosAdicionales = x?.CostosAdicionales != null ? x.CostosAdicionales.Value : 0,
                        fecha = x.Fecha,
                        formaPago = x?.IdFormaPagoNavigation?.TipoFormaPago != null ? x?.IdFormaPagoNavigation?.TipoFormaPago : "Compra en local",
                        idVenta = x.IdVenta,
                        referencia = x.Referencia,
                        status = x.IdStatus.Value,
                        motivoCostosAdicional = x.MotivoCostosAdicional,
                        total = x.Total.Value
                    };
                    response.Add(value);
                }

                dataSalesReportDetail.pendiente = response
                                                .Where(x => x.status == 1)
                                                .ToList();

                dataSalesReportDetail.atendido = response
                                                .Where(x => x.status == 2)
                                                .ToList();
                dataSalesReportDetail.rechazado = response
                                                .Where(x => x.status == 3)
                                                .ToList();

                return dataSalesReportDetail;
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
        public async Task<ReportResponse<List<FieldReportAmountEntity>>> GetTopTenProductsMostSalesAndDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            { 
                return await IReportRepository.GetTopTenProductsMostSalesAndDates(timeStart, timeEnd);

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de productos más vendidos");
            }
        }
        
        public async Task<List<ProductReportDetail>> GetAllProductsMostSalesByCategoryAndDatesDetail(DateTime timeStart, DateTime timeEnd, int idCategory)
        {
            try
            {
                return await IReportRepository.GetAllProductsMostSalesByCategoryAndDatesDetail(timeStart, timeEnd, idCategory);

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
        public async Task<List<MiltiFieldReportEntity>> GetAllLogsModulesGraficsByDates(List<int> idModules, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                return await IReportRepository.GetAllLogsModulesGraficsByDates(idModules, timeStart, timeEnd);

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de reporte por modulos");
            }
        }
        public async Task<List<ModulesReports>> GetAllListModules()
        {
            try
            {
                return await IReportRepository.GetAllListModules();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de modulos");
            }
        }
        public async Task<ModulesReportDetail> GetAllLogsModulesByDatesDetail(List<int> idModules, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                return await IReportRepository.GetAllLogsModulesByDatesDetail(idModules, timeStart, timeEnd);

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de logs para documento por modulos");
            }
        }
        public async Task<List<FieldReportEntity>> GetSimpleMembershipReport()
        {
            try
            {
                return await IReportRepository.GetSimpleMembershipReport();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de mimebros");
            }
        }

        public async Task<List<MembershipUserDetailEntity>> GetDetailMembershipReport()
        {
            try
            {
                return await IReportRepository.GetDetailMembershipReport();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte detalle de mimebros");
            }
        }

        public async Task<List<FieldReportAmountEntity>> GetAllSalesToAdms(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                return await IReportRepository.GetAllSalesToAdms(timeStart, timeEnd);
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de vendedores y valores vendidos");
            }
        }

        public async Task<List<SaleDetailAdmReportEntity>> GetDetailSalesToAdms(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                return await IReportRepository.GetDetailSalesToAdms(timeStart, timeEnd);
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de vendedores y valores vendidos");
            }
        }


    }
}

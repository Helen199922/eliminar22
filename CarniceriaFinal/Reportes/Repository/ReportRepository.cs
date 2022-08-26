using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarniceriaFinal.Reportes.Repository
{
    public class ReportRepository : IReportRepository
    {
        public readonly DBContext Context;
        protected readonly IOptions<ModulesConfiguration> modules;
        public ReportRepository(DBContext _Context, IOptions<ModulesConfiguration> Imodules)
        {
            Context = _Context;
            this.modules = Imodules;
        }

        //Reporte de ventas por fecha
        public async Task<ReportResponse<List<FieldReportEntity>>> GetAllSalesToReportByDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sales = await Context.Venta.ToListAsync();
                var salesSelected = sales
                    .Where(x =>
                                DateTime.Compare(x.Fecha.Value, timeStart) >= 0
                                && DateTime.Compare(x.Fecha.Value, timeEnd) <= 0
                    )
                    .ToList();

                ReportResponse<List<FieldReportEntity>> report = new();
                List<FieldReportEntity> lista = new();
                var totalProducts = 0;

                FieldReportEntity Pendiente = new()
                {
                    name = "Pendiente",
                    value = salesSelected.Where(x => x.IdStatus == 1).ToList().Count()
                };
                totalProducts += Pendiente.value;
                lista.Add(Pendiente);

                FieldReportEntity Atendido = new()
                {
                    name = "Atendido",
                    value = salesSelected.Where(x => x.IdStatus == 2).ToList().Count()
                };
                totalProducts += Atendido.value;
                lista.Add(Atendido);

                FieldReportEntity Rechazado = new()
                {
                    name = "Rechazado",
                    value = salesSelected.Where(x => x.IdStatus == 3).ToList().Count()
                };
                totalProducts += Rechazado.value;
                lista.Add(Rechazado);

                report.totalData = totalProducts;
                report.dataReport = lista;
                return report;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de ventas por fecha");
            }
        }
        public async Task<List<Ventum>> GetDetailSalesToReportByDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sales = await Context.Venta
                    .Include(x => x.IdClienteNavigation.IdPersonaNavigation)
                    .Include(x => x.IdFormaPagoNavigation)
                    .Include(x => x.IdCiudadNavigation)
                    .Include(x => x.IdStatusNavigation)
                    .ToListAsync();
                var salesSelected = sales
                    .Where(x =>
                                DateTime.Compare(x.Fecha.Value, timeStart) >= 0
                                && DateTime.Compare(x.Fecha.Value, timeEnd) <= 0
                    )
                    .ToList();
                
                return salesSelected;
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
                var productsInCategory = await Context.Productos
                    .Where(x => x.CategoriaInProductos.Where(y => y.IdCategoria == idCategory).FirstOrDefault() != null)
                    .AsNoTracking()
                    .ToListAsync();


                var details = await Context.DetalleVenta
                    .Include(x => x.IdVentaNavigation)
                    .ToListAsync();


                //ventas con los productos seleccionados por categoria
                var salesDetails = details
                    .Where(x =>
                                DateTime.Compare(x.IdVentaNavigation.Fecha.Value, timeStart) >= 0
                                && DateTime.Compare(x.IdVentaNavigation.Fecha.Value, timeEnd) <= 0
                                && productsInCategory
                                    .Select(id => id.IdProducto)
                                    .Contains(x.IdProducto.Value)
                    );


                //Seperar las ventas por las categorias
                ReportResponse<List<MiltiFieldReportEntity>> report = new();
                List<MiltiFieldReportEntity> lista = new List<MiltiFieldReportEntity>();

                var totalProducts = 0;
                foreach (var product in productsInCategory)
                {
                    MiltiFieldReportEntity element = new();
                    element.series = new();
                    element.name = product.Titulo;


                    FieldReportEntity pendiente = new()
                    {
                        name = "Pendiente",
                        value = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 1
                                ).ToList().Count()
                    };
                    totalProducts += pendiente.value;
                    element.series.Add(pendiente);

                    FieldReportEntity atendido = new()
                    {
                        name = "Atendido",
                        value = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 2
                                ).ToList().Count()
                    };
                    totalProducts += atendido.value;
                    element.series.Add(atendido);

                    FieldReportEntity rechazado = new()
                    {
                        name = "Rechazado",
                        value = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 3
                        ).ToList().Count()
                    };
                    totalProducts += rechazado.value;
                    element.series.Add(rechazado);

                    lista.Add(element);
                }

                report.totalData = totalProducts;
                report.dataReport = lista;
                return report;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de productos");
            }
        }
        public async Task<List<ProductReportDetail>> GetAllProductsMostSalesByCategoryAndDatesDetail(DateTime timeStart, DateTime timeEnd, int idCategory)
        {
            try
            {
                var productsInCategory = await Context.Productos
                    .Where(x => x.CategoriaInProductos.Where(y => y.IdCategoria == idCategory).FirstOrDefault() != null)
                    .AsNoTracking()
                    .ToListAsync();


                var details = await Context.DetalleVenta
                    .Include(x => x.IdVentaNavigation)
                    .ToListAsync();


                //ventas con los productos seleccionados por categoria
                var salesDetails = details
                    .Where(x =>
                                DateTime.Compare(x.IdVentaNavigation.Fecha.Value, timeStart) >= 0
                                && DateTime.Compare(x.IdVentaNavigation.Fecha.Value, timeEnd) <= 0
                                && productsInCategory
                                    .Select(id => id.IdProducto)
                                    .Contains(x.IdProducto.Value)
                    );


                //Seperar las ventas por las categorias
                //ReportResponse<List<MiltiFieldReportEntity>> report = new();
                List<ProductReportDetail> lista = new List<ProductReportDetail>();

                var totalProducts = 0;
                foreach (var product in productsInCategory)
                {
                    MiltiFieldReportEntity element = new();
                    element.series = new();
                    element.name = product.Titulo;


                    lista.Add(new ProductReportDetail()
                    {
                        idProducto = product.IdProducto,
                        titulo = product.Titulo,
                        ventasAtendidas = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 2
                                ).ToList().Count(),
                        ventasRechazadas = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 3
                                ).ToList().Count(),
                        ventasPendientes = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 1
                                ).ToList().Count(),
                        stock = product.Stock,
                        precio = product.Precio.Value

                    });

                }
                return lista;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Reporte de productos");
            }
        }

        //Reporte de ventas por fecha
        public async Task<List<CategoriesReports>> GetListCategories()
        {
            try
            {
                var categories = await Context.CategoriaProductos.ToListAsync();

                return categories.Select(x => new CategoriesReports()
                {
                    id = x.IdCategoria,
                    value = x.Titulo
                }).ToList();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de categorias");
            }
        }

        //Obtener ids de Modulos Logs
        public async Task<List<ModulesReports>> GetAllListModules()
        {
            try
            {
                return this.modules.Value.Modules.Select(x => new ModulesReports()
                {
                    id = x.idModule,
                    value = x.moduleName
                }).DistinctBy(x => x.id).ToList();
                var categories = await Context.CategoriaProductos.ToListAsync();

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de modulos");
            }
        }

        public async Task<List<MiltiFieldReportEntity>> GetAllLogsModulesGraficsByDates(List<int> idModules, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var response = await Context.Logs
                    .Where(x => idModules.Contains(x.IdModulo))
                    .ToListAsync();


                var logs = response.Where(x => DateTime.Compare(x.Timestamp, timeStart) >= 0
                                                && DateTime.Compare(x.Timestamp, timeEnd) <= 0
                                          ).ToList();

                List<MiltiFieldReportEntity> report = new();

                foreach (var module in idModules)
                {
                    MiltiFieldReportEntity onlyReport = new();
                    FieldReportEntity fine = new();
                    FieldReportEntity error = new();
                    onlyReport.series = new();

                    onlyReport.name = this.modules.Value.Modules.Where(x => x.idModule == module).FirstOrDefault().moduleName;
                    fine.name = "Correcta";
                    fine.value = logs.Where(x => x.IdModulo == module && x.EstadoHttp <= 200).ToList().Count();

                    error.name = "Error";
                    error.value = logs.Where(x => x.IdModulo == module && x.EstadoHttp >= 400).ToList().Count();

                    onlyReport.series.Add(fine);
                    onlyReport.series.Add(error);

                    report.Add(onlyReport);
                }

                return report;

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de logs por modulos");
            }
        }
        public async Task<ModulesReportDetail> GetAllLogsModulesByDatesDetail(List<int> idModules, DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var response = await Context.Logs
                    .Where(x => idModules.Contains(x.IdModulo))
                    .ToListAsync();


                var logs = response.Where(x => DateTime.Compare(x.Timestamp, timeStart) >= 0
                                                && DateTime.Compare(x.Timestamp, timeEnd) <= 0
                                          ).ToList();

                ModulesReportDetail report = new();
                report.correcta = new();
                report.error = new();


                foreach (var module in idModules)
                {
                    List<LogsEntity> fine = new();
                    List<LogsEntity> error = new();

                    fine = logs.Where(x => x.IdModulo == module && x.EstadoHttp <= 200).ToList().Select(x => new LogsEntity()
                    {
                        endpoint = x.Endpoint,
                        estadoHTTP = x.EstadoHttp,
                        hostname = x.Hostname,
                        idModulo = x.IdModulo,
                        mensaje = x.Mensaje,
                        metodo = x.Metodo,
                        modulo = x.Modulo,
                        pathEndPoint = x.PathEndpoint,
                        timestamp = x.Timestamp,
                        idUser = x.IdUser,
                        idLog = x.IdLog
                    }).ToList();

                    error = logs.Where(x => x.IdModulo == module && x.EstadoHttp >= 400).ToList().Select(x => new LogsEntity()
                    {
                        endpoint = x.Endpoint,
                        estadoHTTP = x.EstadoHttp,
                        hostname = x.Hostname,
                        idModulo = x.IdModulo,
                        mensaje = x.Mensaje,
                        metodo = x.Metodo,
                        modulo = x.Modulo,
                        pathEndPoint = x.PathEndpoint,
                        timestamp = x.Timestamp,
                        idLog = x.IdLog,
                        idUser = x.IdUser,
                    }).ToList();


                    report.correcta.AddRange(fine);
                    report.error.AddRange(error);
                }

                return report;

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de logs para documento por modulos");
            }
        }
    }
}

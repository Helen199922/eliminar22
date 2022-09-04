using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

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



                    FieldReportEntity atendido = new()
                    {
                        name = product.Titulo,
                        value = salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto
                                    && x.IdVentaNavigation.IdStatus == 2
                                ).ToList().Count()
                    };
                    totalProducts += atendido.value;
                    element.series.Add(atendido);

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

        public async Task<ReportResponse<List<FieldReportAmountEntity>>> GetTopTenProductsMostSalesAndDates(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var products = await Context.Productos
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
                                && x.IdVentaNavigation.IdStatus == 2
                    );


                //Seperar las ventas por las categorias
                ReportResponse<List<FieldReportAmountEntity>> report = new();

                List<FieldReportAmountEntity> list = new();

                var totalProducts = 0;
                foreach (var product in products)
                {
                    FieldReportAmountEntity element = new();
                    element.name = product.Titulo;
                    element.value = (float)salesDetails.Where(x =>
                                    x.IdProducto == product.IdProducto).Sum(x => x.Precio);

                   list.Add(element);
                }

                var reportOrder = list.OrderByDescending(x => x.value).Take(10).ToList();

                report.totalData = 0;
                report.dataReport = reportOrder;
                return report;
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


        public async Task<List<FieldReportEntity>> GetSimpleMembershipReport()
        {
            try
            {
                var members = await Context.MembresiaInUsuarios.Where(x => x.Status == 1)
                    .Include(x => x.IdMembresiaNavigation)
                    .Include(x => x.IdUsuarioNavigation)
                    .ToListAsync();

                List<FieldReportEntity> fields = new();

                fields.Add(new FieldReportEntity()
                {
                    name = "Oro",
                    value = members.Where(x => x.IdMembresia == 3).ToList().Count()
                });
                fields.Add(new FieldReportEntity()
                {
                    name = "Plata",
                    value = members.Where(x => x.IdMembresia == 2).ToList().Count()
                });

                fields.Add(new FieldReportEntity()
                {
                    name = "Bronce",
                    value = members.Where(x => x.IdMembresia == 1).ToList().Count()
                });

                return fields;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de miembros");
            }
        }


        public async Task<List<MembershipUserDetailEntity>> GetDetailMembershipReport()
        {
            try
            {
                var members = await Context.MembresiaInUsuarios.Where(x => x.Status == 1)
                    .Include(x => x.IdMembresiaNavigation)
                    .Include(x => x.IdUsuarioNavigation)
                    .ToListAsync();

                List<MembershipUserDetailEntity> fields = members.Select(x => new MembershipUserDetailEntity()
                {
                    usuario = x.IdUsuarioNavigation.Username,
                    FechaFin = x.FechaFin,
                    FechaInicio = x.FechaInicio,
                    IdUsuario = x.IdUsuario,
                    CantProductosComprados = x.CantProductosComprados,
                    membresiaTitulo = x.IdMembresiaNavigation.Titulo
                }).ToList();


                return fields;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Lista de miembros");
            }
        }


        public async Task<List<FieldReportAmountEntity>> GetAllSalesToAdms(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                var sales = await Context.Venta.Where(x => x.IdAdm != null)
                    .Include(x => x.IdAdmNavigation)
                    .AsNoTracking()
                    .ToListAsync();

                var salesAvailables = sales
                .Where(x =>
                    DateTime.Compare(x.Fecha.Value, timeStart) >= 0
                    && DateTime.Compare(x.Fecha.Value, timeEnd) <= 0
                    && x.IdStatus == 2
                ).ToList();

                var userAdmsSales = await Context.Usuarios
                    .Where(x => x.IdRol == 1 || x.IdRol == 2)
                    .AsNoTracking()
                    .ToListAsync();


                List<FieldReportAmountEntity> fields = new();
                foreach (var user in userAdmsSales)
                {
                    var saleTotal = salesAvailables.Where(y =>
                                    y.IdAdm == user.IdUsuario)
                                    .Sum(x => x.Total);

                    fields.Add(new FieldReportAmountEntity()
                    {
                        name = user.Username,
                        value = (float)Math.Round(saleTotal == null ? 0 : saleTotal.Value, 2)

                    
                    });
                }

                return fields;
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
                var sales = await Context.Venta.Where(x => x.IdAdm != null)
                    .Include(x => x.IdAdmNavigation)
                    .Include(x => x.DetalleVenta)
                    .ThenInclude(x => x.IdProductoNavigation)
                    .Include(x => x.IdClienteNavigation)
                    .ThenInclude(x => x.IdPersonaNavigation)
                    .AsNoTracking()
                    .ToListAsync();

                var salesAvailables = sales
                .Where(x =>
                    DateTime.Compare(x.Fecha.Value, timeStart) >= 0
                    && DateTime.Compare(x.Fecha.Value, timeEnd) <= 0
                    && x.IdStatus == 2
                ).ToList();

                var userAdmsSales = await Context.Usuarios
                    .Where(x => x.IdRol == 1 || x.IdRol == 2)
                    .Include(x => x.IdPersonaNavigation)
                    .AsNoTracking()
                    .ToListAsync();


                List<SaleDetailAdmReportEntity> fields = new();
                foreach (var user in userAdmsSales)
                {
                    var salesInUserAdm = salesAvailables.Where(y =>
                                    y.IdAdm == user.IdUsuario).ToList();

                    foreach (var sale in salesInUserAdm)
                    {
                        string saleDetail = "";
                        foreach (var z in sale.DetalleVenta)
                        {
                            var motivoDscto = z.IdMembresiaInUsuario != null
                                                    ? "Membresía "
                                                    : z.IdPromocion != null
                                                        ? "Promoción "
                                                        : "NO";

                            saleDetail = saleDetail + String
                                .Format(" [Producto: {0}, Cantidad: {1}, Descuento: {2}, Motivo_Descuento: {3}], "
                                , z.IdProductoNavigation.Titulo
                                , z.Cantidad
                                , z.Descuento != null ? z.Descuento.Value.ToString() : "NO"
                                , motivoDscto);
                        }

                        fields.Add(new SaleDetailAdmReportEntity()
                        {
                            cedulaCliente = sale.IdClienteNavigation.IdPersonaNavigation.Cedula,
                            FechaAceptacionVenta = sale.FechaFinal.Value,
                            montoTotal = (float)Math
                                        .Round(sale.Total.Value == null ? 0 : sale.Total.Value, 2),
                            usuarioAdministrador = user.Username,
                            detalleVenta = saleDetail
                        });
                    }
                }

                return fields;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("detalle de las ventas");
            }
        }
    }
}

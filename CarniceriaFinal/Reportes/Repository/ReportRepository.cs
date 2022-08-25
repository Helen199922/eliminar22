using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Reportes.DTOs;
using CarniceriaFinal.Reportes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Reportes.Repository
{
    public class ReportRepository : IReportRepository
    {
        public readonly DBContext Context;
        public ReportRepository(DBContext _Context)
        {
            Context = _Context;
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
                    .Where(x => x.SubInCategoria.Where(y => y.IdCategoria == idCategory).FirstOrDefault() != null)
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
    }
}

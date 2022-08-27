using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Sales.Repository
{
    public class SalesAdmRepository : ISalesAdmRepository
    {
        public readonly DBContext Context;
        public SalesAdmRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<List<SalesAdmEntity>> GetAllSales(int idStatus)
        {
            try
            {
                return await Context.Venta
                    .Where(s => s.IdStatus == idStatus)
                    .Select(x => new SalesAdmEntity()
                    {
                        idVenta = x.IdVenta,
                        cedula = x.IdClienteNavigation.IdPersonaNavigation.Cedula,
                        fecha = x.Fecha,
                        total = x.Total.Value,
                        status = x.IdStatus.Value,
                        costosAdicionales = x.CostosAdicionales,
                        motivoCostosAdicional = x.MotivoCostosAdicional,
                        idFormaPago = x.IdFormaPago.Value,
                        formaPago = x.IdFormaPagoNavigation.TipoFormaPago,
                        hasDiscount = x.DetalleVenta.Where(y => y.IdPromocion != null || y.IdMembresiaInUsuario != null)
                                        .ToList().Count > 0 ? true : false
                    })
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Estado de la venta");
            }
        }
        public async Task<Ventum> GetSalesById(int idSale)
        {
            try
            {
                return await Context.Venta
                    .Where(s => s.IdVenta == idSale)
                    .Include(x => x.IdStatusNavigation)
                    .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Detaille de la venta");
            }
        }
        public async Task<int> GetUserIdByIdSale(int idSale)
        {
            try
            {
                return await Context.Usuarios
                    .Where(x => x.IdPersona == (Context.Venta
                                .Where(s => s.IdVenta == idSale)
                                .Include(x => x.IdClienteNavigation)
                                .ThenInclude(x => x.IdPersonaNavigation)
                                .Select(x => x.IdClienteNavigation.IdPersonaNavigation.IdPersona)
                                .FirstOrDefault())
                    && x.IdRol == 3)
                    .Select(x => x.IdUsuario).FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("obtener información sobre la venta");
            }
        }
        public async Task<List<VentaStatus>> GetSalesStatusAdm()
        {
            try
            {

                var result = await Context.VentaStatuses
                    .Where(x => x.IdVentaStatus != 1)
                    .ToListAsync();
                
                if (result == null)
                    throw RSException.NoData("Tuvimos un problema al obtener la información");

                return result;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Status de la venta");
            }
        }
        public async Task<List<DetailSaleAdmEntity>> GetDetailByIdSale(int idSale)
        {
            try
            {
                var result = await Context.DetalleVenta
                    .Where(s => s.IdVenta == idSale)
                    .Select(x => new DetailSaleAdmEntity()
                    {
                        cantidad = x.Cantidad.Value,
                        precio = x.Precio.Value,
                        idProduct = x.IdProducto.Value,
                        idDetalle = x.IdDetalleVenta,
                        idPromocion = x.IdPromocion,
                        idMembresiaInUser = x.IdMembresiaInUsuario,
                        stockActual = x.IdProductoNavigation.Stock,
                        descuento = x.Descuento.Value,
                        titulo = x.IdProductoNavigation.Titulo,
                        detailDiscount = (x.IdMembresiaInUsuario != null || x.IdPromocion != null)
                            ? new DetailDiscountEntity()
                            {
                                titulo = x.IdMembresiaInUsuario != null ? x.IdMembresiaInUsuarioNavigation.IdMembresiaNavigation.Titulo : x.IdPromocionNavigation.Titulo,
                                fechaInicio = x.IdMembresiaInUsuario != null ? x.IdMembresiaInUsuarioNavigation.FechaInicio : x.IdPromocionNavigation.FechaInicio,
                                fechaFin = x.IdMembresiaInUsuario != null ? x.IdMembresiaInUsuarioNavigation.FechaFin : x.IdPromocionNavigation.FechaFin,
                                ultimaActualizacion = x.IdPromocionNavigation.FechaUpdate == null ? x.IdPromocionNavigation.FechaUpdate : null,
                                status = x.IdMembresiaInUsuario != null ? x.IdMembresiaInUsuarioNavigation.Status : x.IdPromocionNavigation.Status
                            }
                            : null
                    })
                    .ToListAsync();

                if (result == null) return new();
                else return result;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Detalle de la venta");
            }
        }
        public async Task<Producto> GetInfoProductByIdProduct(int idProduct)
        {
            try
            {
                return await Context.Productos
                    .Where(s => s.IdProducto == idProduct)
                    .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Información sobre el producto");
            }
        }
        public async Task<Ventum> attendSaleByIdSale(int idSale)
        {
            try
            {
                var sale = await Context.Venta
                    .Where(s => s.IdVenta == idSale)
                    .Include(x => x.DetalleVenta)
                    .Include(x => x.IdClienteNavigation.IdPersonaNavigation)
                    .FirstOrDefaultAsync();
                sale.IdStatus = 2;
                await Context.SaveChangesAsync();
                return sale;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Atender venta");
            }
        }
        public async Task<Boolean> declineSaleByIdSale(int idSale)
        {
            try
            {
                var sale = await Context.Venta
                    .Where(s => s.IdVenta == idSale)
                    .FirstOrDefaultAsync();
                sale.IdStatus = 3;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Rechazar venta");
            }
        }
        public async Task<Boolean> pendingSaleByIdSale(int idSale)
        {
            try
            {
                var sale = await Context.Venta
                    .Where(s => s.IdVenta == idSale)
                    .FirstOrDefaultAsync();
                sale.IdStatus = 1;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Venta pendiente");
            }
        }
    }
}

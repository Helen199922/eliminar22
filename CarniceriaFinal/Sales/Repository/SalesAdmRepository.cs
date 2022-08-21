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
                        hasDiscount = x.DetalleVenta.Where(y => y.IdPromocion != null || y.IdMembresia != null).ToList().Count > 0 ? true : false
                    })
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Estado de la venta");
            }
        }
        //public async Task<List<DetailSaleAdmEntity>> GetDetailByIdSale(int idSale)
        //{
        //    try
        //    {
        //        return await Context.DetalleVenta
        //            .Where(s => s.IdVenta == idSale)
        //            .Select(x => new DetailSaleAdmEntity()
        //            {
        //                cantidad = x.Cantidad.Value,
        //                precio = x.Precio.Value,
        //                idPromocion = x.IdPromocion,
        //                idMembresia = x.IdMembresia,
        //                descuento = x.Descuento.Value,
        //                titulo = x.IdProductoNavigation.Titulo,
        //                detailDiscount = (x.IdMembresia != null || x.IdPromocion != null) 
        //                    ? new DetailDiscountEntity()
        //                        {
        //                            titulo = x.IdMembresia != null ? x.IdMembresiaNavigation.Titulo : x.IdPromocionNavigation.Titulo,
        //                            fechaInicio = x.IdMembresia != null ? x.IdMembresiaNavigation.fe : x.IdPromocionNavigation.Titulo,
        //                            fechaFin = x.IdMembresia != null ? x.IdMembresiaNavigation.Titulo : x.IdPromocionNavigation.Titulo,
        //                            ultimaActualizacion = x.IdMembresia != null ? x.IdMembresiaNavigation.Titulo : x.IdPromocionNavigation.Titulo,
        //                            status = x.IdMembresia != null ? x.IdMembresiaNavigation.Titulo : x.IdPromocionNavigation.Titulo
        //                    }
        //                    : null
        //                })
        //            .ToListAsync();
        //    }
        //    catch (Exception err)
        //    {
        //        throw RSException.ErrorQueryDB("Estado de la venta");
        //    }
        //}
    }
}

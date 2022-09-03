using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.Repository
{
    public class SaleRepository : ISaleRepository
    {
        public readonly DBContext Context;
        private readonly IConfiguration Configuration;
        public SaleRepository(DBContext _Context, IConfiguration configuration)
        {
            Context = _Context;
            Configuration = configuration;
        }
        public async Task<Ventum> CreateSale(Ventum sale)
        {
            try
            {
                sale.FechaFinal = DateTime.Now.AddMilliseconds(int.Parse(Configuration["AppConstants:MiliSegToDisableSale"]));

                await Context.Venta.AddAsync(sale);

                await Context.SaveChangesAsync();
                return sale;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("registrar Venta");
            }
        }

        public async Task<DetalleVentum> CreateDetail(DetalleVentum detail)
        {
            try
            {
                await Context.DetalleVenta.AddAsync(detail);
                await Context.SaveChangesAsync();
                return detail;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("registrar detalle de la Venta");
            }
        }

        public async Task<List<Ventum>> GetAllSalesPending()
        {
            try
            {
                return await Context.Venta.Where(x => x.IdStatus == 1).ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las ventas pendientes");
            }
        }
        public async Task<Ventum> FindSaleById(int idSale)
        {
            try
            {
                return await Context.Venta.AsNoTracking().Where(x => x.IdVenta == idSale).FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener la venta especificada.");
            }
        }
        public async Task<string> UpdateSale(Ventum sale)
        {
            try
            {
                Context.Venta.Update(sale);
                await Context.SaveChangesAsync();
                return "Se ha actualizado correctamente";
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar la venta.");
            }
        }

        public async Task<List<VentaStatus>> GetAllSalesStatus()
        {
            try
            {
                return await Context.VentaStatuses.ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener el estado de las ventas.");
            }
        }

        public async Task<List<Ventum>> GetAllSales()
        {
            try
            {
                return await Context.Venta.Include(x => x.DetalleVenta)
                    .Where(x => x.IdStatus != 4).ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las ventas.");
            }
        }
        public async Task<Ventum> GetStatusByIdSale(int idSale)
        {
            try
            {
                return await Context.Venta.Include(x => x.DetalleVenta)
                    .Where(x => x.IdVenta == idSale).FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener el detalle de la venta.");
            }
        }
        public async Task<List<Ventum>> FindAllSalesByIdClient(int idClient)
        {
            try
            {
                return await Context.Venta
                    .Where(x => (x.IdCliente == idClient) && x.IdStatus != 4 && x.IdStatus != 3)
                    .Include(x => x.DetalleVenta).ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las ventas por cliente.");
            }
        }

        public async Task<Ventum> FindCompleteSaleByIdSale(int idSale)
        {
            try
            {
                return await Context
                        .Venta
                        .Where(x => x.IdVenta == idSale)
                        .Include(x => x.DetalleVenta)
                        .ThenInclude(x => x.IdProductoNavigation)
                        .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener detalle de la venta especificada.");
            }
        }

        public async Task<List<Ventum>> FindAllCompleteSaleByIdClient(int idClient)
        {
            try
            {
                return await Context
                        .Venta
                        .Where(x => x.IdClienteNavigation.IdCliente == idClient && x.IdStatus == 2 && x.IsAuthUser == 1)
                        .Include(x => x.DetalleVenta)
                        .ThenInclude(x => x.IdProductoNavigation)
                        .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener lista detalle de las ventas del cliente.");
            }
        }
        public async Task<Ventum> getStatusOfSaleByIdSale(int idSale)
        {
            try
            {
                return await Context
                        .Venta
                        .Where(x => x.IdVenta == idSale)
                        .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener el estatus de la venta.");
            }
        }
    }
}

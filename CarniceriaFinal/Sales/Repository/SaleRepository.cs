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
        public SaleRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<Ventum> CreateSale(Ventum sale)
        {
            try
            {
                await Context.Venta.AddAsync(sale);
                //Context.Entry(sale.IdClienteNavigation).State = EntityState.Detached;
                //Context.ChangeTracker.DetectChanges();
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
    }
}

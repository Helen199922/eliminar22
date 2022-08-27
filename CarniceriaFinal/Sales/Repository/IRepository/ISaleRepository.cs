using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.IRepository
{
    public interface ISaleRepository
    {
        Task<Ventum> CreateSale(Ventum sale);
        Task<DetalleVentum> CreateDetail(DetalleVentum detail);
        Task<List<Ventum>> GetAllSalesPending();
        Task<Ventum> FindSaleById(int idSale);
        Task<string> UpdateSale(Ventum sale);
        Task<List<VentaStatus>> GetAllSalesStatus();
        Task<List<Ventum>> GetAllSales();
        Task<List<Ventum>> FindAllSalesByIdClient(int idClient);
        Task<Ventum> GetStatusByIdSale(int idSale);
    }
}

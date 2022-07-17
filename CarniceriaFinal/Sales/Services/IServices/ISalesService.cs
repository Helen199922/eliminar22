using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.IServices
{
    public interface ISalesService
    {
        Task<string> CreateSaleNoUser(SaleNoUserRequestEntity sale);
        Task<ModelsEF.Cliente> CreateClient(ClientEntity clientNewData, int idPerson);
        Task<Persona> CreatePerson(ClientEntity clientNewData);
        Task<ModelsEF.Cliente> UpdateClientNoUser(ClientEntity clientNewData, ModelsEF.Cliente client);
        Task<Persona> UpdatePersonNoUser(ClientEntity clientNewData, Persona person);
        Task<Ventum> CreateSale(SaleEntity saleData, int idClient);
        Task<DetalleVentum> CreateDetail(SaleDetailEntity detail, int idSale);
        Task<string> CreateSaleUser(SaleNoUserRequestEntity sale);
        Task<string> UpdateStateSale(int IdStatus, int idSale);
        Task<List<SalesStatus>> GetAllSalesStatus();
        Task<List<SalesCompleteEntity>> GetAllSales();
        Task<List<SalesCompleteEntity>> GetAllSalesByIdClient(int idClient);
    }
}

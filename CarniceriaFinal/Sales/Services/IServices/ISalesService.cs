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
        Task<SalesUserInformationResponse> CreateSaleNoUser(SaleNoUserRequestEntity sale);
        Task<ModelsEF.Cliente> CreateClient(int idPerson);
        Task<Persona> CreatePerson(ClientEntity clientNewData);
        Task<ModelsEF.Cliente> UpdateClientNoUser(ClientEntity clientNewData, ModelsEF.Cliente client);
        Task<Persona> UpdatePersonNoUser(ClientEntity clientNewData, Persona person);
        Task<Ventum> CreateSale(int? idClient, float finalAmount, SaleNoUserRequestEntity saleInfo);
        Task<DetalleVentum> CreateDetail(SaleDetailEntity detail, int idSale);
        Task<SalesUserInformationResponse> CreateSaleUser(SaleNoUserRequestEntity sale);
        Task<string> UpdateStateSale(int IdStatus, int idSale);
        Task<List<SalesStatus>> GetAllSalesStatus();
        Task<List<SalesCompleteEntity>> GetAllSales();
        Task<List<SalesCompleteEntity>> GetAllSalesByIdClient(int idClient);
        Task<SalesUserInformationResponse> GetStatusByIdSale(int idSale);
        Task<SaleDetailCotizacionEntity> FindCompleteSaleByIdSale(int idSale);
        Task<List<SaleDetailCotizacionEntity>> FindAllCompleteSaleByIdClient(int idUser);
        Task<int> getStatusOfSaleByIdSale(int idSale);
    }
}

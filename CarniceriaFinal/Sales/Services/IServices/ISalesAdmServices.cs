using CarniceriaFinal.Sales.DTOs;

namespace CarniceriaFinal.Sales.Services.IServices
{
    public interface ISalesAdmServices
    {
        Task<List<SalesAdmEntity>> GetAllSales(int idStatus);
        Task<ResumeSaleDetail> GetDetailByIdSale(int idSale);
        Task<string> attendSale(int idSale);
        Task<string> declineSale(int idSale);
        Task<SaleAdmRequestSaleDetail> GetSaleDetailById(int idSale);
    }
}

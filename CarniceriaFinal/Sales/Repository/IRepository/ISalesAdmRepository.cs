using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;

namespace CarniceriaFinal.Sales.Repository.IRepository
{
    public interface ISalesAdmRepository
    {
        Task<List<SalesAdmEntity>> GetAllSales(int idStatus);
        Task<List<DetailSaleAdmEntity>> GetDetailByIdSale(int idSale);
        Task<Producto> GetInfoProductByIdProduct(int idProduct);
        Task<Ventum> GetSalesById(int idSale);
        Task<int> GetUserIdByIdSale(int idSale);
        Task<List<VentaStatus>> GetSalesStatusAdm();
        Task<Boolean> attendSaleByIdSale(int idSale);
        Task<Boolean> declineSaleByIdSale(int idSale);
        Task<Boolean> pendingSaleByIdSale(int idSale);
    }
}

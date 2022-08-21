using CarniceriaFinal.Sales.DTOs;

namespace CarniceriaFinal.Sales.Services.IServices
{
    public interface ISalesAdmServices
    {
        Task<List<SalesAdmEntity>> GetAllSales(int idStatus);
    }
}

using CarniceriaFinal.Sales.DTOs;

namespace CarniceriaFinal.Sales.Repository.IRepository
{
    public interface ISalesAdmRepository
    {
        Task<List<SalesAdmEntity>> GetAllSales(int idStatus);
    }
}

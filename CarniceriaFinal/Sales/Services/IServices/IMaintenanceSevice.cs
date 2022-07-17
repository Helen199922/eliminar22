using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Services.IServices
{
    public interface IMaintenanceSevice
    {
        Task<List<int>> getPendingSalesIDs();
    }
}

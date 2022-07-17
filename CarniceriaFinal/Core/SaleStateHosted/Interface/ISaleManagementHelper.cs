using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.SaleStateHosted.Interface
{
    public interface ISaleManagementHelper
    {
        List<SaleEntity> getPendingSalesIDs(List<Ventum> sales);
        Ventum SaleEntityToUpdate(SaleEntity sale);
    }
}

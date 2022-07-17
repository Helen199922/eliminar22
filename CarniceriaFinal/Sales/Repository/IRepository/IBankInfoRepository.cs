using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Repository.IRepository
{
    public interface IBankInfoRepository
    {
        Task<List<InfoBancarium>> GetBankInfo();
    }
}

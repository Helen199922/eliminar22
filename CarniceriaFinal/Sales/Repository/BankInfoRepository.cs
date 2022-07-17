using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Repository
{
    public class BankInfoRepository : IBankInfoRepository
    {
        public readonly DBContext Context;
        public BankInfoRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<List<InfoBancarium>> GetBankInfo()
        {
            try
            {
                return await Context.InfoBancaria
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Información Bancaria");
            }
        }
    }
}

using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        public readonly DBContext Context;
        public PromotionRepository(DBContext _Context)
        {
            Context = _Context;
        }

        public async Task<Promocion> CreatePromotion(Promocion promotion)
        {
            try
            {
                await Context.Promocions.AddAsync(promotion);
                await Context.SaveChangesAsync();
                return promotion;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Promocion");
            }
        }

        public async Task<List<Promocion>> GetAll()
        {
            try
            {
                return await Context.Promocions.ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("List Promocion");
            }
        }
    }
}

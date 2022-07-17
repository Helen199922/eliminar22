using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Interfaces.IRepository
{
    public interface IPromotionRepository
    {
        Task<List<Promocion>> GetAll();
        Task<Promocion> CreatePromotion(Promocion promotion);
    }
}

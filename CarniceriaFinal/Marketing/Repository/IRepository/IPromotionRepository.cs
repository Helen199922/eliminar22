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
        Task<Promocion> getLastPromotion();
        Task<Promocion> CreatePromotion(Promocion promotion);
        Task<Promocion> UpdatePromotion(Promocion promotion);
        Task<Promocion> StatusPromotion(int status, int idPromotion);
        Task<Promocion> PromotionIsActivate(int idPromotion);

    }
}

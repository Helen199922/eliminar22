using CarniceriaFinal.Marketing.DTOs;
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
        Task<Promocion> getLastPromotion(DateTime? upperLimit, DateTime? lowerLimit);
        Task<Promocion> CreatePromotion(Promocion promotion);
        Task<Promocion> UpdatePromotion(Promocion promotion);
        Task<Promocion> StatusPromotion(int status, int idPromotion);
        Task<Promocion> PromotionIsActivate(int idPromotion);
        Task<List<PromotionProductEntity>> GetAllProductsByIdPromotion(int? idPromotion);
        Task<Boolean> UpdateListPruductsInPromo(List<int> pruductsInPromo, int idPromotion);
        Task<Promocion> getLastPromotionByIdPromotion(DateTime? upperLimit, DateTime? lowerLimit, int idPromotion);
        Task<List<PorcentajeDscto>> GetAllDsctPromotion();
        Task<Promocion> GetPromotionById(int idPromotion);
    }
}

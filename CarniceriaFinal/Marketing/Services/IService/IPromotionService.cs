using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Interfaces.IService
{
    public interface IPromotionService
    {
        Task<List<PromotionEntity>> GetAll();
        Task<string> CreatePromotion(PromotionEntity promotionDetail);
        Task<string> UpdatePromotion(PromotionEntity promotionDetail);
        Task<string> StatusPromotion(int status, int idPromotion);

    }
}

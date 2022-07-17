using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Interfaces.IService
{
    public interface IPromotionService
    {
        Task<string> CreatePromotion(Promocion promotion);
        Task<List<Promocion>> GetAll();

    }
}

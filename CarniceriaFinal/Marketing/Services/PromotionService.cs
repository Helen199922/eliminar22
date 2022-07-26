using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.Marketing.Interfaces.IService;
using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository IPromotionRepository;
        public PromotionService(IPromotionRepository IPromotionRepository)
        {
            this.IPromotionRepository = IPromotionRepository;
        }
        public async Task<string> CreatePromotion(Promocion promotion)
        {
            try
            {
                await IPromotionRepository.CreatePromotion(promotion);
                return "Promoción creada correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la promoción.");
            }

        }

        public async Task<List<Promocion>> GetAll()
        {
            try
            {
                return await IPromotionRepository.GetAll();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista de promociones.");
            }

        }

    }
}

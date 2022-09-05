using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
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
        private readonly IMapper IMapper;
        public PromotionService(IPromotionRepository IPromotionRepository, IMapper IMapper)
        {
            this.IPromotionRepository = IPromotionRepository;
            this.IMapper = IMapper;
        }
        public async Task<List<PromotionEntity>> GetAll()
        {
            try
            {
                return IMapper.Map<List<PromotionEntity>>(await IPromotionRepository.GetAll());
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
        public async Task<string> CreatePromotion(PromotionEntity promotionDetail)
        {
            try
            {
                promotionDetail.FechaUpdate = DateTime.Now;
                Promocion promo = IMapper.Map<Promocion>(promotionDetail);

                var lastPromotion = await IPromotionRepository.getLastPromotion(promotionDetail.fechaFin, promotionDetail.fechaInicio);
                if (lastPromotion != null)
                    throw RSException.BadRequest(String
                        .Format("Ya existe una promoción activa. Por favor, espere a que termine o desactívela: {0}",
                        lastPromotion.Titulo));


                Promocion promoCreated = await IPromotionRepository.CreatePromotion(promo);
                await UpdateListPruductsInPromo(promotionDetail.pruductsInPromo, promoCreated.IdPromocion);
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
        public async Task<string> UpdatePromotion(PromotionEntity promotionDetail)
        {
            try
            {
                promotionDetail.FechaUpdate = DateTime.Now;
                Promocion promo = IMapper.Map<Promocion>(promotionDetail);

                if(promotionDetail.Status == 1)
                {
                    var lastPromotion = await IPromotionRepository.getLastPromotionByIdPromotion(promotionDetail.fechaFin, promotionDetail.fechaInicio, promotionDetail.idPromocion.Value);
                    if (lastPromotion != null)
                        throw RSException.BadRequest(String
                            .Format("Ya existe una promoción activa. Por favor, espere a que termine o desactívela: {0}",
                            lastPromotion.Titulo));
                }

                await IPromotionRepository.UpdatePromotion(promo);

                await UpdateListPruductsInPromo(promotionDetail.pruductsInPromo, promotionDetail.idPromocion.Value);
                return "Promoción actualizada correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar la promoción.");
            }

        }
        public async Task<string> StatusPromotion(int status, int idPromotion)
        {
            try
            {
                await IPromotionRepository.StatusPromotion(status, idPromotion);
                return "Estado de promoción actualizada correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el estado de la promoción.");
            }

        }
        public async Task<String> isAvailabilityToActivatePromotion(IsAvailabilityCreatePromoEntity data)
        {
            try
            {
                var isAvailability = false;
                if (data.idPromocion == null) return "Ha ocurrido un error con la promoción. Por favor intente pronto.";

                var promotionData = await IPromotionRepository.GetPromotionById(data.idPromocion.Value);

                if (promotionData.Status == 1) return "";

                var response = await IPromotionRepository
                    .getLastPromotionByIdPromotion(data.fechaFin, data.fechaInicio, data.idPromocion.Value);


                return response == null ? "" : String
                        .Format("Ya existe una promoción activa. Por favor, espere a que termine o desactívela: {0}",
                        response.Titulo);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al consultar estado de promociones.");
            }

        }
        public async Task<String> isAvailabilityToCreatePromotion(IsAvailabilityCreatePromoEntity data)
        {
            try
            {
                var isAvailability = false;
  

                if(data.idPromocion != null)
                {
                    var promotionData = await IPromotionRepository.GetPromotionById(data.idPromocion.Value);
                    if (promotionData.Status == 0) return "";
                }



                var response = data.idPromocion != null
                    ? await IPromotionRepository.getLastPromotionByIdPromotion(data.fechaFin, data.fechaInicio, data.idPromocion.Value)
                    : await IPromotionRepository.getLastPromotion(data.fechaFin, data.fechaInicio);

                return response == null ? "" : String
                        .Format("Ya existe una promoción activa. Por favor, espere a que termine o desactívela: {0}",
                        response.Titulo);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al consultar estado de promociones.");
            }

        }
        public async Task<List<PromotionProductEntity>> GetAllProductsByIdPromotion(int? idPromotion)
        {
            try
            {
                var response = await IPromotionRepository.GetAllProductsByIdPromotion(idPromotion);
                if (response == null)
                    return new();

                return response.DistinctBy(x => x.idProduct).ToList();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista de productos para promociones.");
            }

        }
        private async Task<Boolean> UpdateListPruductsInPromo(List<int> pruductsInPromo, int idPromotion)
        {
            try
            {
                await IPromotionRepository.UpdateListPruductsInPromo(pruductsInPromo, idPromotion);

                return true;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la guardar la lista de productos para promoción.");
            }

        }
        public async Task<List<PorcentajeDscto>> GetAllDsctPromotion()
        {
            try
            {
                return await IPromotionRepository.GetAllDsctPromotion();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista de dsctos.");
            }

        }
    }
}

﻿using AutoMapper;
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

                await IPromotionRepository.CreatePromotion(promo);
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

                await IPromotionRepository.UpdatePromotion(promo);
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
    }
}

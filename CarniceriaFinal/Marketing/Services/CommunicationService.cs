using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.Marketing.Interfaces.IService;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ICommunicationRepository ICommunicationRepository;
        private readonly IMapper IMapper;
        public CommunicationService(ICommunicationRepository commRepo, IMapper IMapper)
        {
            this.ICommunicationRepository = commRepo;
            this.IMapper = IMapper;
        }
        //public async Task<string> CreatePromotion(Promocion promotion)
        //{
        //    try
        //    {
        //        await IPromotionRepository.CreatePromotion(promotion);
        //        return "Promoción creada correctamente";
        //    }
        //    catch (RSException err)
        //    {
        //        throw new RSException(err.TypeError, err.Code, err.MessagesError);
        //    }
        //    catch (Exception)
        //    {
        //        throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la promoción.");
        //    }

        //}

        public async Task<List<CommunicationEntity>> GetAll()
        {
            try
            {
                var commRepo = await ICommunicationRepository.GetAll();

                List<CommunicationEntity> comm = new();
                foreach (var item in commRepo)
                {
                    var valroe= IMapper.Map<CommunicationEntity>(
                            item
                        );
                    CommunicationEntity communication = valroe;


                    TypeCommunicationEntity typeComm =
                        IMapper.Map<TypeCommunicationEntity>(
                            item.IdTipoComunicacionNavigation
                        );
                    communication.TipoComunicacion = typeComm;
                    comm.Add(communication);
                }
                return comm;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista de comunicaciones.");
            }

        }
    }
}

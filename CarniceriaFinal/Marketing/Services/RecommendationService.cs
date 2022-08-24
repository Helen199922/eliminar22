using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Repository;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository IRecommendationRepository;
        private readonly IMapper IMapper;
        public RecommendationService(IRecommendationRepository IRecommendationRepository, IMapper IMapper)
        {
            this.IRecommendationRepository = IRecommendationRepository;
            this.IMapper = IMapper;
        }
        public async Task<List<TimesToEatEntity>> GetAllTimesToEat()
        {
            try
            {
                return IMapper.Map<List<TimesToEatEntity>>(await IRecommendationRepository.GetAllTimesToEat());
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista momentos de degustación.");
            }
        }
        public async Task<List<TimesToEatEntity>> GetAllTimesToEatGeneralUser()
        {
            try
            {
                return IMapper.Map<List<TimesToEatEntity>>(await IRecommendationRepository.GetAllTimesToEatGeneralUser());
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la lista momentos de degustación.");
            }
        }
        public async Task<TimesToEatEntity> ChangeStatusTimesToEat(int idTimesToEat, Boolean status)
        {
            try
            {
                int statusValue = status ? 1 : 0;
                return IMapper.Map<TimesToEatEntity>(await IRecommendationRepository.ChangeStatusTimesToEat(idTimesToEat, statusValue));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el estado del modo de preparación.");
            }
        }
        public async Task<List<PreparationWay>> GetAllPreparationWays()
        {
            try
            {
                return IMapper.Map<List<PreparationWay>>(await IRecommendationRepository.GetAllPreparationWays());
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la manera de preparar el producto.");
            }
        }
        public async Task<List<PreparationWay>> GetAllPreparationWaysGeneralUser(int idTimeToEat)
        {
            try
            {
                return IMapper.Map<List<PreparationWay>>(await IRecommendationRepository.GetAllPreparationWaysGeneralUser(idTimeToEat));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la manera de preparar el producto.");
            }
        }

        public async Task<PreparationWay> ChangeStatusPreparationWays(int idPreparationWay, Boolean status)
        {
            try
            {
                int statusValue = status ? 1 : 0;
                return IMapper.Map<PreparationWay>(await IRecommendationRepository.ChangeStatusPreparationWays(idPreparationWay, statusValue));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el estado del modo de preparación.");
            }
        }

        public async Task<CreateTimesToEatWay> GetTimeToEatDetail(int idTimeToEat)
        {
            try
            {
                CreateTimesToEatWay result = new CreateTimesToEatWay();
                
                var response = await IRecommendationRepository.GetTimeToEatDetail(idTimeToEat);
                result = IMapper.Map<CreateTimesToEatWay>(response);
                result.productIds = response.MomentoDegustacionInProductos.Select(x => x.IdProducto).ToList();


                return result;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el detalle para degustación.");
            }
        }
        public async Task<CreatePreparationWay> GetPreparationWayDetail(int idPreparationWay)
        {
            try
            {
                CreatePreparationWay result = new CreatePreparationWay();

                var response = await IRecommendationRepository.GetPreparationWayDetail(idPreparationWay);
                if (response == null) {
                    throw RSException.NoData("No se ha encotrado información");
                }
                result = IMapper.Map<CreatePreparationWay>(response);
                result.productIds = response.PreparacionProductoInProductos.Select(x => x.IdProducto.Value).ToList();


                return result;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el detalle para modo de preparación.");
            }
        }
        public async Task<PreparationWay> CreatePreparationWay(CreatePreparationWay data)
        {
            try
            {
                data.Status = 1;
                if (data.productIds == null)
                    data.productIds = new();

                    var preparation = IMapper.Map<PreparacionProducto>(data);

                return IMapper.Map<PreparationWay>(await IRecommendationRepository.CreatePreparationWay(preparation, data.productIds));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la forma de preparación del producto.");
            }
        }
        public async Task<TimesToEatEntity> CreateTimeToEat(CreateTimesToEatWay data)
        {
            try
            {
                if (data.productIds == null)
                    data.productIds = new();

                var timesToEat = IMapper.Map<MomentoDegustacion>(data);

                return IMapper.Map<TimesToEatEntity>(await IRecommendationRepository.CreateTimeToEat(timesToEat, data.productIds));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear el momento de consumo del producto.");
            }
        }
        public async Task<PreparationWay> UpdatePreparationWay(CreatePreparationWay data)
        {
            try
            {
                if (data.productIds == null)
                    data.productIds = new();

                var preparation = IMapper.Map<PreparacionProducto>(data);

                return IMapper.Map<PreparationWay>(await IRecommendationRepository.UpdatePreparationWay(preparation, data.productIds));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar la forma de preparación del producto.");
            }
        }
        public async Task<TimesToEatEntity> UpdateTimeToEat(CreateTimesToEatWay data)
        {
            try
            {
                if (data.productIds == null)
                    data.productIds = new();

                var preparation = IMapper.Map<MomentoDegustacion>(data);

                return IMapper.Map<TimesToEatEntity>(await IRecommendationRepository.UpdateTimeToEat(preparation, data.productIds));
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el momento de preparación del producto.");
            }
        }

        public async Task<List<SpecialEventEntity>> GetAllSpecialEvent()
        {
            try { 

                var response = await IRecommendationRepository.GetAllSpecialEvent();
                return IMapper.Map<List<SpecialEventEntity>>(response);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al la lista de eventos especiales.");
            }
        }
        public async Task<string> CreateSpecialEvent(SpecialEventEntity specialEvent)
        {
            try
            {
                var specialEventMap = IMapper.Map<EventoEspecial>(specialEvent);
                var isValidCreated = await IRecommendationRepository.isValidCreateSpecialEvent(specialEventMap);
                if (!isValidCreated)
                {
                    return "No se ha podido crear el evento especial, asegúrese de que no esté activo otro evento en este mismo horario.";
                }

                var response = await IRecommendationRepository.CreateSpecialEvent(specialEventMap);

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear el evento especial.");
            }
        }
        public async Task<string> UpdateSpecialEvent(SpecialEventEntity specialEvent)
        {
            try
            {
                var specialEventMap = IMapper.Map<EventoEspecial>(specialEvent);
                var isValidCreated = await IRecommendationRepository.isValidUpdateSpecialEvent(specialEventMap);
                if (!isValidCreated)
                {
                    return "No se ha podido actualizar el evento especial, asegúrese de que no esté activo otro evento en este mismo horario.";
                }

                var response = await IRecommendationRepository.UpdateSpecialEvent(specialEventMap);

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear el evento especial.");
            }
        }
        public async Task<string> DisableSpecialEvent(int idSpecialEvent)
        {
            try
            {
                var response = await IRecommendationRepository.DisableSpecialEvent(idSpecialEvent);

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear el desactivar el evento especial.");
            }
        }
        public async Task<string> EnableSpecialEvent(int idSpecialEvent)
        {
            try
            {
                var isValidCreated = await IRecommendationRepository.isValidActivateSpecialEvent(idSpecialEvent);
                if (!isValidCreated)
                {
                    return "No se ha podido activar el evento especial, asegúrese de que no esté activo otro evento en este mismo horario.";
                }

                var response = await IRecommendationRepository.EnableSpecialEvent(idSpecialEvent);

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al activar el evento especial.");
            }
        }
    }
}

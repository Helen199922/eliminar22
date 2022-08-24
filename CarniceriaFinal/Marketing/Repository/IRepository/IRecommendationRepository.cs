using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Repository.IRepository
{
    public interface IRecommendationRepository
    {
        Task<List<MomentoDegustacion>> GetAllTimesToEatGeneralUser();
        Task<MomentoDegustacion> ChangeStatusTimesToEat(int idTimesToEat, int status);
        Task<List<PreparacionProducto>> GetAllPreparationWaysGeneralUser(int idTimeToEat);
        Task<PreparacionProducto> ChangeStatusPreparationWays(int idPreparationWay, int status);
        Task<List<MomentoDegustacion>> GetAllTimesToEat();
        Task<List<PreparacionProducto>> GetAllPreparationWays();
        Task<PreparacionProducto> CreatePreparationWay(PreparacionProducto preparation, List<int> productIds);
        Task<MomentoDegustacion> CreateTimeToEat(MomentoDegustacion tomesToEat, List<int> preparationProducts);
        Task<MomentoDegustacion> GetTimeToEatDetail(int idTimeToEat);
        Task<PreparacionProducto> GetPreparationWayDetail(int idPreparationWay);
        Task<PreparacionProducto> UpdatePreparationWay(PreparacionProducto preparation, List<int> productIds);
        Task<MomentoDegustacion> UpdateTimeToEat(MomentoDegustacion tomesToEat, List<int> preparationProducts);
        //Crear Recomendació
        Task<EventoEspecial> GetAllSpecialEventByIdEvent(int idEvent);
        Task<List<EventoEspecial>> GetAllSpecialEvent();
        Task<EventoEspecial> CreateSpecialEvent(EventoEspecial specialEvent);
        Task<EventoEspecial> UpdateSpecialEvent(EventoEspecial specialEvent);
        Task<EventoEspecial> DisableSpecialEvent(int idSpecialEvent);
        Task<EventoEspecial> EnableSpecialEvent(int idSpecialEvent);
        //isValidCreateSpecialEven
        Task<Boolean> isValidCreateSpecialEvent(EventoEspecial specialEvent);
        //isValidActivateSpecialEven
        Task<Boolean> isValidActivateSpecialEvent(int idSpecialEvent);
        Task<Boolean> isValidUpdateSpecialEvent(EventoEspecial specialEvent);
    }
}

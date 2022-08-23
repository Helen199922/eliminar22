﻿using CarniceriaFinal.Marketing.DTOs;

namespace CarniceriaFinal.Marketing.Services.IService
{
    public interface IRecommendationService
    {
        Task<List<TimesToEatEntity>> GetAllTimesToEat();
        Task<CreateTimesToEatWay> GetTimeToEatDetail(int idTimeToEat);
        Task<List<PreparationWay>> GetAllPreparationWays();
        Task<List<TimesToEatEntity>> GetAllTimesToEatGeneralUser();
        Task<TimesToEatEntity> ChangeStatusTimesToEat(int idTimesToEat, Boolean status);
        Task<List<PreparationWay>> GetAllPreparationWaysGeneralUser(int idTimeToEat);
        Task<PreparationWay> ChangeStatusPreparationWays(int idPreparationWay, Boolean status);
        Task<PreparationWay> CreatePreparationWay(CreatePreparationWay data);
        Task<TimesToEatEntity> CreateTimeToEat(CreateTimesToEatWay data);
        Task<PreparationWay> UpdatePreparationWay(CreatePreparationWay data);
        Task<TimesToEatEntity> UpdateTimeToEat(CreateTimesToEatWay data);
        Task<CreatePreparationWay> GetPreparationWayDetail(int idPreparationWay);
    }
}

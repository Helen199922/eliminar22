using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Repository.IRepository
{
    public interface ICitiesRepository
    {
        Task<List<Provincium>> GetCitiesCost();
        Task<string> UpdateCitiesCost(List<CityCost> cities);
        Task<Ciudad> GetCityById(int idCity);
    }
}

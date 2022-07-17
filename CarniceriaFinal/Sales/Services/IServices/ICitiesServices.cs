using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Sales.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Services.IServices
{
    public interface ICitiesServices
    {
        Task<List<ProvincesCityCost>> GetCitiesCost();
        Task<List<ProvincesCityCost>> UpdateCitiesCost(List<CityCost> cities);
        Task<float> GetCityCostById(int idCity);
    }
}

using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Repository.IRepository;
using CarniceriaFinal.Sales.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Services
{
    public class CitiesServices : ICitiesServices
    {
        private readonly IMapper IMapper;
        public readonly ICitiesRepository ICitiesRepository;
        public CitiesServices(IMapper IMapper, ICitiesRepository ICitiesRepository)
        {
            this.ICitiesRepository = ICitiesRepository;
            this.IMapper = IMapper;

        }
        public async Task<List<ProvincesCityCost>> GetCitiesCost()
        {
            try
            {
                var citiesCost = await ICitiesRepository.GetCitiesCost();
                List<ProvincesCityCost> provinces = new();
                foreach (var city in citiesCost)
                {
                    var pronvince = city.IdProvinciaNavigation;
                    provinces.Add(new ProvincesCityCost
                    {
                        label = pronvince.Provincia,
                        value = pronvince.Provincia,
                        items = new()
                    });

                    foreach (var item in pronvince.Ciudads)
                    {
                        var cityCost = new CityCostEntity
                        {
                            label = item.Ciudad1,
                            value = item.CostoEnvio == null ? 0 : item.CostoEnvio.Value,
                            title = item.IdCiudad
                        };
                        provinces.Last().items.Add(cityCost);
                    }
                }


                return provinces;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los costos de las ciudades.");
            }
        }
        public async Task<float> GetCityCostById(int idCity)
        {
            try
            {
                var city = await ICitiesRepository.GetCityById(idCity);
                if (city == null)
                    throw RSException.BadRequest("Ciudad no encontrada");

                return  city.CostoEnvio.Value;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el costo de la ciudad.");
            }
        }
        public async Task<List<ProvincesCityCost>> UpdateCitiesCost(List<CityCost> cities)
        {
            try
            {
                var citiesCost = await ICitiesRepository.UpdateCitiesCost(cities);
                return await this.GetCitiesCost();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los costos de las ciudades.");
            }
        }
    }
}

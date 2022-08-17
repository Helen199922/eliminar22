using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Repository
{
    public class CitiesRepository : ICitiesRepository
    {
        public readonly DBContext Context;
        public CitiesRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<List<Provincium>> GetCitiesCost()
        {
            try
            {
                return await Context.Provincia
                    .Include(x => x.Ciudads)
                    .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Costo por ciudad");
            }
        }
        public async Task<Ciudad> GetCityById(int idCity)
        {
            try
            {
                return await Context.Ciudads
                    .Where(x => x.IdCiudad == idCity)
                    .FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Ciudad por id");
            }
        }
        public async Task<string> UpdateCitiesCost(List<CityCost> cities)
        {
            try
            {
                foreach (var item in cities)
                {
                    var citySelected = await Context.Ciudads
                        .Where(x => x.IdCiudad == item.idCiudad)
                    .FirstOrDefaultAsync();

                    if (citySelected == null)
                        throw RSException.NoData("No se ha encontrado la ciudad");

                    citySelected.CostoEnvio = item.costoEnvio;
                    await Context.SaveChangesAsync();
                }
                return "Proceso realizado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar costo por ciudad");
            }
        }
    }
}

using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        public readonly DBContext Context;
        public UnidadMedidaRepository(DBContext _Context)
        {
            Context = _Context;
        }

        public Task<UnidadMedidum> GetUnidadMedidaById(int id)
        {
            try
            {
                return Context.UnidadMedida.Where(x => x.IdUnidad == id).FirstOrDefaultAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Unidad de Medida");
            }

        }
        public async Task<List<UnidadMedidum>> GetAllMeasureUnit()
        {
            try
            {
                var units = await Context.UnidadMedida.ToListAsync();
                return units;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Get Unidad de Medida");
            }

        }
        public async Task<UnidadMedidum> CreateMeasureUnit(string unit)
        {
            try
            {
                UnidadMedidum measureUnit = new();
                measureUnit.Unidad = unit;
                await Context.UnidadMedida.AddAsync(measureUnit);
                await Context.SaveChangesAsync();
                return measureUnit;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear Unidad de Medida");
            }

        }
    }
}

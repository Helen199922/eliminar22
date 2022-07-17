using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.IRepository
{
    public interface IUnidadMedidaRepository
    {
        Task<UnidadMedidum> GetUnidadMedidaById(int id);
        Task<List<UnidadMedidum>> GetAllMeasureUnit();
        Task<UnidadMedidum> CreateMeasureUnit(string unit);
    }
}

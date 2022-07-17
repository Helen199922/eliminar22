using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.IServicios
{
    public interface IMeasureUnitService
    {
        Task<string> CreateMeasureUnit(string unit);
        Task<List<MeasureUnitEntity>> GetAllMeasureUnit();
    }
}

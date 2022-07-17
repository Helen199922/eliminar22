using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Servicios
{
    public class MeasureUnitService : IMeasureUnitService
    {
        private IUnidadMedidaRepository IUnidadMedidaRepository;
        public MeasureUnitService(IUnidadMedidaRepository IUnidadMedidaRepository)
        {
            this.IUnidadMedidaRepository = IUnidadMedidaRepository;
        }
        public async Task<string> CreateMeasureUnit(string unit)
        {
            try
            {
                await IUnidadMedidaRepository.CreateMeasureUnit(unit);
                        
                return "Unidad guardada correctamente";

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }
        public async Task<List<MeasureUnitEntity>> GetAllMeasureUnit()
        {
            try
            {
                var measureUnit =  await IUnidadMedidaRepository.GetAllMeasureUnit();
                MeasureUnitEntity unit;
                List<MeasureUnitEntity> listUnit = new();
                foreach (var item in measureUnit)
                {
                    unit = new();
                    unit.idUnidad = item.IdUnidad;
                    unit.unidad = item.Unidad;
                    listUnit.Add(unit);
                }
                return listUnit;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }
    }
}

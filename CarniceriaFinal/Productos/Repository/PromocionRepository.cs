using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public class PromocionRepository : IPromocionRepository
    {
        public readonly DBContext Context;
        public PromocionRepository(DBContext _Context)
        {
            Context = _Context;
        }

        public Task<Promocion> GetPromocionById(int id)
        {
            try
            {
                return Context.Promocions.Where(x => x.IdPromocion == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Promoción");
                
            }

        }
    }
}

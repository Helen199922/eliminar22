using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Repository
{
    public class CommunicationRepository : ICommunicationRepository
    {
        public readonly DBContext Context;
        public CommunicationRepository(DBContext _Context)
        {
            Context = _Context;
        }

        //public async Task<Promocion> CreateComunication(Promocion promotion)
        //{
        //    try
        //    {
        //        await Context.Promocions.AddAsync(promotion);
        //        await Context.SaveChangesAsync();
        //        return promotion;
        //    }
        //    catch (Exception)
        //    {
        //        throw RSException.ErrorQueryDB("Promocion");
        //    }
        //}

        public async Task<List<Comunicacion>> GetAll()
        {
            try
            {
                return await Context.Comunicacions
                    .Include(x => x.IdTipoComunicacionNavigation)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("List Promocion");
            }
        }
    }
}

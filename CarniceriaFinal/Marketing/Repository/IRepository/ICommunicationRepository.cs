using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Repository.IRepository
{
    public interface ICommunicationRepository
    {
        public Task<List<Comunicacion>> GetAll();
    }
}

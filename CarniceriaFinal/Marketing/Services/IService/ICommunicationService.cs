using CarniceriaFinal.Marketing.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Marketing.Services.IService
{
    public interface ICommunicationService
    {
        public Task<List<CommunicationEntity>> GetAll();
    }
}

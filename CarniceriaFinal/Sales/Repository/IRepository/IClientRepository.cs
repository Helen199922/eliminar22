using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.IRepository
{
    public interface IClientRepository
    {
        Task<ModelsEF.Cliente> GetClientByIdentification(string indentification);
        Task<ModelsEF.Cliente> UpdateClient(ModelsEF.Cliente client);
        Task<ModelsEF.Cliente> CreateClient(ModelsEF.Cliente client);
        Task<ModelsEF.Cliente> GetClientByIdUser(int idUser);
    }
}

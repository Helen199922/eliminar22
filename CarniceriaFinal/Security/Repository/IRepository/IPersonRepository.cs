using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.IRepository
{
    public interface IPersonRepository
    {
        Task<Persona> GetPersonByIdentification(string identification);
        Task<Persona> CreatePerson(Persona persona);
        Task<Persona> UpdatePerson(Persona person);
        Task<Usuario> FindUserByIdentification(string identification);
    }
}

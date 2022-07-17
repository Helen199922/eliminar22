using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Security.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Security.Repository
{
    public class PersonRepository : IPersonRepository
    {
        public readonly DBContext Context;
        public PersonRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<Persona> GetPersonByIdentification(string identification)
        {
            try
            {
                return await Context.Personas.Where(x => x.Cedula == identification).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener usuario por identificación");
            }
        }

        public async Task<Persona> CreatePerson(Persona persona)
        {
            try
            {
                await Context
                    .Personas.AddAsync(persona);
                await Context.SaveChangesAsync();
                return persona;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Crear Persona");
            }
        }
        public async Task<Persona> UpdatePerson(Persona person)
        {
            try
            {
                Context.Personas.Update(person);
                await Context.SaveChangesAsync();
                return person;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Persona");
            }
        }

        public async Task<Usuario> FindUserByIdentification(string identification)
        {
            try
            {
                return await Context.Usuarios
                    .Where(x => x.IdPersonaNavigation.Cedula == identification)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener usuario por identificación");
            }
        }

    }
}

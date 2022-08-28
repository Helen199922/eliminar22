using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Cliente.Repository
{
    public class ClientRepository : IClientRepository
    {
        public readonly DBContext Context;
        public ClientRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<ModelsEF.Cliente> GetClientByIdentification(string indentification)
        {
            try
            {
               return await Context.Clientes.Where(x => x.IdPersonaNavigation.Cedula == indentification)
                    .Select(x => new ModelsEF.Cliente
                    {
                        Direccion = x.Direccion,
                        IdCliente = x.IdCliente,
                        IdCiudad = x.IdCiudad,
                        IdPersona = x.IdPersona,
                        IdPersonaNavigation = x.IdPersonaNavigation,
                        Telefono1 = x.Telefono1,
                        Telefono2 = x.Telefono2,
                        Referencia = x.Referencia,
                        Venta = x.Venta
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener cliente por identificación");
            }
        }
        public async Task<ModelsEF.Cliente> UpdateClient(ModelsEF.Cliente client)
        {
            try
            {
                Context.Clientes.Update(client);
                await Context.SaveChangesAsync();
                return client;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Actualizar cliente: ");
            }
        }
        public async Task<ModelsEF.Cliente> CreateClient(ModelsEF.Cliente client)
        {
            try
            {
                await Context.Clientes.AddAsync(client);
                await Context.SaveChangesAsync();
                return client;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Crear cliente");
            }
        }
        public async Task<ModelsEF.Cliente> GetClientByIdUser(int idUser)
        {
            try
            {
                //obtener persona
                var person = await Context
                    .Personas
                    .Where(x => x.Usuarios.Where(y => y.IdUsuario == idUser && y.IdRol == 3).FirstOrDefault() != null)
                    .Include(x => x.Clientes)
                    .FirstOrDefaultAsync();

                if (person == null)
                    return null;

                return person.Clientes.FirstOrDefault();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener cliente cliente");
            }
        }
    }
}

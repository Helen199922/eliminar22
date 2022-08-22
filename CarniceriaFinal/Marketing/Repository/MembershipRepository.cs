using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Sales.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Marketing.Repository
{
    public class MembershipRepository : IMembershipRepository
    {
        public readonly ISalesAdmRepository ISalesAdmRepository;
        public readonly DBContext Context;
        public MembershipRepository(DBContext _Context, ISalesAdmRepository iSalesAdmRepository)
        {
            Context = _Context;
            this.ISalesAdmRepository = iSalesAdmRepository;
        }

        public async Task<Membresium> GetMembershipDetail(int idMembershipInUser)
        {
            try
            {

                var member = await Context.Membresia
                    .Where(x => x.Status == 1 && x.MembresiaInUsuarios
                            .Where(y => y.IdMembresiaInUsuario == idMembershipInUser).FirstOrDefault() != null
                    )
                    .FirstOrDefaultAsync();

                return member;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía por id de usuario.");
            }
        }
        public async Task<List<Membresium>> GetAllMembershipDetail()
        {
            try
            {

                var member = await Context.Membresia
                    .Where(x => x.Status == 1)
                    .ToListAsync();

                return member;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresías.");
            }
        }
        public async Task<MembresiaInUsuario> GetMembershipDetailToAdmSales(int idMembershipInUser)
        {
            try
            {

                var member = await Context.MembresiaInUsuarios
                    .Where(x => x.IdMembresiaInUsuario == idMembershipInUser)
                    .Include(x => x.IdMembresiaNavigation)
                    .FirstOrDefaultAsync();

                return member;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía por id de usuario.");
            }
        }
        public async Task<Boolean> RegisterMembershipToUser(int idUser, Membresium membership)
        {
            try
            {
                   await Context.MembresiaInUsuarios.AddAsync(new() {
                        IdUsuario = idUser,
                        IdMembresia = membership.IdMembresia,
                        FechaFin = DateTime.Now.AddDays(membership.DuracionMembresiaDias),
                        FechaInicio = DateTime.Now,
                        CantProductosComprados = 0,
                        Status = 1
                    });

                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía agregada de forma incorrecta.");
            }
        }
        public async Task<Boolean> BlockMembershipToUser(int idMembershipInUser)
        {
            try
            {
                var member = await Context.MembresiaInUsuarios
                    .Where(x => x.IdMembresiaInUsuario == idMembershipInUser)
                    .FirstOrDefaultAsync();

                member.Status = 3;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía bloqueada de forma incorrecta.");
            }
        }
        public async Task<Boolean> AdministrationMembershipTimes()
        {
            try
            {
                var members = await Context.MembresiaInUsuarios.Where(x => x.Status == 1).ToListAsync();

                members.ForEach(member =>
                {
                    if (DateTime.Compare(member.FechaFin, DateTime.Now) < 0)
                        member.Status = 0;
                });
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        public async Task<Boolean> AdmMemberTimesByIdUserInMembership(int idMembresiaInUser)
        {
            try
            {
                var members = await Context
                    .MembresiaInUsuarios
                    .Where(x => x.Status == 1 && x.IdMembresiaInUsuario == idMembresiaInUser)
                    .Include(x => x.IdMembresiaNavigation)
                    .ToListAsync();

                members.ForEach(member =>
                {
                    if(member.CantProductosComprados >= member.IdMembresiaNavigation.CantProductosMembresia)
                        member.Status = 0;

                    if (DateTime.Compare(member.FechaFin, DateTime.Now) < 0)
                        member.Status = 0;
                });
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        
        public async Task<MemberShipCommEntity> GetMembershipHome()
        {
            try
            {
                var members = await Context.MembresiaInUsuarios.Where(x => x.Status == 1)
                    .Include(x => x.IdMembresiaNavigation)
                    .Include(x => x.IdUsuarioNavigation)
                    .ToListAsync();

                var oro = members.Where(x => x.IdMembresia == 3).FirstOrDefault();
                var plata = members.Where(x => x.IdMembresia == 2).FirstOrDefault();
                var bronce = members.Where(x => x.IdMembresia == 1).FirstOrDefault();

                return new MemberShipCommEntity()
                {
                    Oro = oro != null ? new MemberCommEntity()
                    {
                        imageMembership = oro.IdMembresiaNavigation.Imagen,
                        memberName = oro.IdUsuarioNavigation.Username,
                        titleMembership = oro.IdMembresiaNavigation.Titulo
                    } : null,
                    Plata = plata != null ? new MemberCommEntity()
                    {
                        imageMembership = plata.IdMembresiaNavigation.Imagen,
                        memberName = plata.IdUsuarioNavigation.Username,
                        titleMembership = plata.IdMembresiaNavigation.Titulo
                    } : null,
                    Bronce = bronce != null ? new MemberCommEntity()
                    {
                        imageMembership = bronce.IdMembresiaNavigation.Imagen,
                        memberName = bronce.IdUsuarioNavigation.Username,
                        titleMembership = bronce.IdMembresiaNavigation.Titulo
                    } : null

                };
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        public async Task<MembresiaInUsuario> GetMembershipByIdUser(int idUser)
        {
            try
            {

                var member = await Context.MembresiaInUsuarios
                    .Where(x => x.Status == 1 && x.IdUsuario == idUser)
                    .Include(x => x.IdMembresiaNavigation)
                    .FirstOrDefaultAsync();
                if (member == null) return null;

                if (DateTime.Compare(member.FechaFin, DateTime.Now) < 0)
                {
                    member.Status = 0;
                    await Context.SaveChangesAsync();

                    return null;
                }

                return member;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía por id de usuario.");
            }
        }
        public async Task<MembresiaInUsuario> GetMembershipByIdClient(int IdClient)
        {
            try
            {
                var user = await Context.Usuarios.Where(x => x.IdPersonaNavigation.Clientes.Where(y => y.IdCliente == IdClient).FirstOrDefault() != null).FirstOrDefaultAsync();
                if (user == null)
                    return null;

                return await this.GetMembershipByIdUser(user.IdUsuario);
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Membresía por id de cliente.");
            }
        }
        public async Task<MembresiaInUsuario> GetLastMembershipByIdUser(int idUser)
        {
            try
            {

                var member = await Context.MembresiaInUsuarios
                    .Where(x => x.IdUsuario == idUser && x.Status != 3)
                    .ToListAsync();

             var newMember = member.Select(d => d)
            .Distinct()
            .OrderByDescending(d => d.FechaFin)
            .Select(d => d).ToList();

                if(newMember == null || newMember.Count == 0)
                {
                    return null;
                }
                return newMember[0];
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener la última membresia del usuario.");
            }
        }
        //Para servicio de administración de ventas (previamente, se debe actualizar la compra como activa)
        public async Task<String> AddProductsToMembershipByIdUser(int idMembresiaInUser, int cantProduct)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var member = await _Context.MembresiaInUsuarios
                   .Where(x => x.IdMembresiaInUsuario == idMembresiaInUser && x.Status != 3)
                   .Include(x => x.IdMembresiaNavigation)
                   .FirstOrDefaultAsync();
                    var valueToAdd = member.CantProductosComprados + cantProduct;

                    if(valueToAdd > member.IdMembresiaNavigation.CantProductosMembresia)
                    {
                        member.CantProductosComprados = member.IdMembresiaNavigation.CantProductosMembresia.Value;

                    }
                    else
                    {
                        member.CantProductosComprados = valueToAdd;
                    }

                    if(member.CantProductosComprados == member.IdMembresiaNavigation.CantProductosMembresia)
                    {
                        member.Status = 0;
                    }

                    await _Context.SaveChangesAsync();
                }

                return "Proceso realizado correctamente";

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Tuvimos un error al registrar la compra con membresía.");
            }
        }
        public async Task<String> RemoveProductsToMembershipByIdUser(int idMembresiaInUser, int cantProduct)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var member = await _Context.MembresiaInUsuarios
                   .Where(x => x.IdMembresiaInUsuario == idMembresiaInUser && x.Status != 3)
                   .Include(x => x.IdMembresiaNavigation)
                   .FirstOrDefaultAsync();
                    var valueToAdd = member.CantProductosComprados - cantProduct;

                    if (valueToAdd < 0)
                    {
                        member.CantProductosComprados = 0;

                    }
                    else
                    {
                        member.CantProductosComprados = valueToAdd;
                    }

                    await _Context.SaveChangesAsync();
                }

                return "Proceso realizado correctamente";

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Tuvimos un error al registrar la compra con membresía.");
            }
        }
        public async Task<float> AmountToMembershipInUser(int idUser, int idClient)
        {
            try
            {
                var lastMembership = await this.GetLastMembershipByIdUser(idUser);

                if (lastMembership == null) return 0;
                if (lastMembership.Status == 1) return 0;

                //Si aún no se cumple el año, debemos mandar 0 de monto
                if (DateTime.Compare(lastMembership.FechaFin.AddYears(1), DateTime.Now) > 0) return 0;


                using (var _Context = new DBContext())
                {
                    var minDate = DateTime.Now.AddYears(-2);
                    if (lastMembership != null)
                        minDate = lastMembership.FechaFin;

                    var salesValue = await _Context.Venta.Where(x => x.IdCliente == idClient && x.IdStatus == 1
                    //Obtenemos todas las ventas mayores a la fecha de finalización de membresía
                    )
                    .AsNoTracking()
                    .ToListAsync();

                    var salesAmount = salesValue.Where(x => DateTime.Compare(minDate.AddYears(1), x.Fecha.Value) <= 0)
                        .Select(x => x.Total).Sum();

                    if (salesAmount == null) return 0;
                    
                    return salesAmount.Value;
                }

            }
            catch (Exception)
            {
                //log de error
                throw RSException.ErrorQueryDB("Tuvimos un error al calcular el momnto de membresía.");
            }
        }
        public async Task<float> AmountToMemberUserDeclineSale(int idUser, int idClient)
        {
            try
            {
                var lastMembership = await this.GetLastMembershipByIdUser(idUser);

                using (var _Context = new DBContext())
                {

                    var salesValue = await _Context.Venta.Where(x => x.IdCliente == idClient && x.IdStatus == 1
                    //Obtenemos todas las ventas mayores a la fecha de finalización de membresía
                    )
                    .AsNoTracking()
                    .ToListAsync();

                    var salesAmount = salesValue.Where(x => DateTime.Compare(lastMembership.FechaInicio.AddYears(-1), 
                        lastMembership.FechaInicio) <= 0)
                        .Select(x => x.Total).Sum();

                    if (salesAmount == null) return 0;

                    return salesAmount.Value;
                }

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Tuvimos un error al re-validar el momnto para acceso a membresía.");
            }
        }
    }
}

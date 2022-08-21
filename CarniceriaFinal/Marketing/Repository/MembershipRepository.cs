using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;

namespace CarniceriaFinal.Marketing.Repository
{
    public class MembershipRepository : IMembershipRepository
    {
        public readonly DBContext Context;
        public MembershipRepository(DBContext _Context)
        {
            Context = _Context;
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
                    .Where(x => x.IdUsuario == idUser)
                    .ToListAsync();

                member.Select(d => d)
            .Distinct()
            .OrderByDescending(d => d.FechaFin)
            .Select(d => d);

                if(member.Count == 0)
                {
                    return null;
                }
                return member[0];
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener la última membresia del usuario.");
            }
        }
        //Para servicio de administración de ventas (previamente, se debe actualizar la compra como activa)
        public async Task<String> AddProductsToMembershipByIdUser(int idUser, int cantProduct)
        {
            try
            {
                //Colocar productos comprados a membresía
                //var member = await this.GetMembershipByIdUser(idUser); (validad que la membresía exista en el servicio, tambien la cantidad maxima)
                //RSException.(); (retornar cuando la membresia no exista)

                using (var _Context = new DBContext())
                {
                    var member = await _Context.MembresiaInUsuarios
                   .Where(x => x.Status == 1 && x.IdUsuario == idUser)
                   .FirstOrDefaultAsync();

                    member.CantProductosComprados = member.CantProductosComprados + cantProduct;
                    await _Context.SaveChangesAsync();
                }

                return "Proceso realizado correctamente";
                    //Crear nueva membresía

                    //Validar si la membresía se aplica correctamente

                    //Eliminar membresía se hace arriba



                

                //Cuando llega el idUser (ya existe, la compra como activa)

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

                if (lastMembership.Status == 1) return 0;

                using (var _Context = new DBContext())
                {
                    var minDate = DateTime.Now.AddYears(-2);
                    if (lastMembership != null)
                        minDate = lastMembership.FechaFin;

                    var salesValue = await _Context.Venta.Where(x => x.IdCliente == idClient
                            && DateTime.Compare(minDate.AddYears(1), DateTime.Now) > 0
                            //Obtenemos todas las ventas mayores a la fecha de finalización de membresía
                    )
                    .AsNoTracking()
                    .ToListAsync();

                    var salesAmount = salesValue.Select(x => x.Total).Sum();

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
        public async Task<String> CreateMembershipByIdUser(int idUser, MembresiaInUsuario membershipDetail)
        {
            try
            {
                using (var _Context = new DBContext())
                {

                    var member = await _Context.MembresiaInUsuarios
                        .AddAsync(new MembresiaInUsuario()
                        {
                            IdUsuario = idUser,
                            FechaFin = membershipDetail.FechaFin,
                            FechaInicio = DateTime.Now,
                            IdMembresia = membershipDetail.IdMembresia,
                            CantProductosComprados = 0,
                            Status = 1
                        });

                    await _Context.SaveChangesAsync();
                }

                return "Proceso realizado correctamente";

            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Tuvimos un error al registrar una membresía.");
            }
        }
    }
}

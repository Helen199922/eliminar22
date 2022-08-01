using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CarniceriaFinal.Marketing.Repository
{
    public class PromotionalEmailRepository : IPromotionalEmailRepository
    {
        public readonly DBContext Context;
        public readonly IPromotionRepository IPromotionRepository;
        public PromotionalEmailRepository(DBContext _Context, IPromotionRepository IPromotionRepository)
        {
            Context = _Context;
            this.IPromotionRepository = IPromotionRepository;
        }

        public async Task<CorreoPromocion> GetEmailByidPromotion(int idPromotion)
        {
            try
            {
                    return await Context.CorreoPromocions
                    .Where(x => x.IdPromocionNavigation.IdPromocion == idPromotion)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        public async Task<CorreoPromocion> CreateEmailByIdPromotion(CorreoPromocion email)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var emails = await _Context.CorreoPromocions
                        .Where(x => x.IdPromocion == email.IdPromocion)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                    if (emails != null)
                        throw RSException.BadRequest("Ya hay una promoción por correo creada. No puede crear otra más.");
                    
                    email.FechaUpdate = DateTime.Now;
                    email.IsSendingEmails = 0;
                    await _Context.CorreoPromocions.AddAsync(email);
                    await _Context.SaveChangesAsync();
                    return email;
                }
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Crear publicidad por correo.");
            }
        }
        public async Task<CorreoPromocion> UpdateEmailByidEmail(CorreoPromocion email)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var newEmail = await _Context.CorreoPromocions
                    .Where(x => x.IdCorreo == email.IdCorreo)
                    .FirstOrDefaultAsync();
                    newEmail.FechaUpdate = DateTime.Now;
                    newEmail.Imagen = email.Imagen;
                    newEmail.UrlPromocion = email.UrlPromocion;
                    newEmail.Titulo = email.Titulo;
                    newEmail.Descripcion = email.Descripcion;

                    await _Context.SaveChangesAsync();
                    return newEmail;
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        public async Task<string> CancelSenderEmailByIdPromotion(int idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var emailPromotion = await _Context.CorreoPromocions
                    .Where(x => x.IdPromocion == idPromotion)
                    .Include(x => x.CorreoPromocionInUsers)
                    .FirstOrDefaultAsync();

                    foreach (var emailInUser in emailPromotion.CorreoPromocionInUsers)
                    {
                        if(emailInUser.IdEstatusEmail == 1)
                        {
                            emailInUser.IdEstatusEmail = 2;
                            emailInUser.FechaUpdate = DateTime.Now;
                        }
                        
                    }
                    await _Context.SaveChangesAsync();
                    return "Proceso realizado correctamente";
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Actualizar estado de correo.");
            }
        }
        public async Task<List<Usuario>> GetUsersToSendEmail(int idCorreoPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    List<Usuario> userAvailable = new();

                    var users = await GetUsersAvailablesToSendEmail(idCorreoPromotion);

                    if (users.Count == 0)
                        return null;

                    List<Task> newPromo = new();
                    var json = JsonSerializer.Serialize(users.Select(x => x.IdUsuario));
                    var option = _Context.CorreoPromocionInUsers
                    .FromSqlRaw("call sp_setStatus_pre_emailSender ({0}, {1})", json, idCorreoPromotion)
                    .AsEnumerable().ToList();

                    return users.Where(x => option.Select(x => x.IdUsuario).Contains(x.IdUsuario)).ToList();
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener usuarios para enviar publicidad por correo.");
            }
        }
        public async Task<Usuario> getUserInSenderEmail(int idUser, int idCorreoPromocion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var user = await _Context.CorreoPromocionInUsers
                        .Where(x => x.IdUsuario == idUser && x.IdCorreoPromocion == idCorreoPromocion)
                        .Include(x => x.IdUsuarioNavigation)
                        .FirstOrDefaultAsync();
                    
                    return user.IdUsuarioNavigation;
                }
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener usuario.");
            }
        }
        public async Task<List<Usuario>> GetUsersToRetryEmailByIdPromotion(int idCorreoPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    List<Usuario> userAvailable = new();

                    var users = await GetUsersAvailablesToRetrySendEmail(idCorreoPromotion);
                    
                    if(users.Count == 0)
                        return null;

                    List<Task> newPromo = new();
                    var json = JsonSerializer.Serialize(users.Select(x => x.IdUsuario));
                    var option = _Context.CorreoPromocionInUsers
                    .FromSqlRaw("call sp_setStatus_pre_emailSender ({0}, {1})", json, idCorreoPromotion)
                    .AsEnumerable().ToList();

                    return users.Where(x => option.Select(x => x.IdUsuario).Contains(x.IdUsuario)).ToList();
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener usuarios para enviar publicidad por correo.");
            }
        }
        public async Task<List<Usuario>> GetUsersAvailablesToSendEmail(int idCorreoPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var users = await _Context.Usuarios
                        .Where(x => (
                                x.Status == 1
                                && x.IdRol == 3
                                && x.ReceiveEmail == 1
                                && x.CorreoPromocionInUsers
                                    .Where(y =>
                                            y.IdUsuario == x.IdUsuario
                                            && y.IdCorreoPromocion == idCorreoPromotion
                                     )
                                    .FirstOrDefault() == null
                         ))
                        .Include(x => x.IdPersonaNavigation)
                        .AsNoTracking()
                        .Take(300)
                        .ToListAsync();

                    return users;
                }
            }
            catch (Exception)
            {
                return new();
            }
        }
        public async Task<List<Usuario>> GetUsersAvailablesToRetrySendEmail(int idCorreoPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var users = await _Context.Usuarios
                        .Where(x => (
                                x.Status == 1
                                && x.IdRol == 3
                                && x.ReceiveEmail == 1
                                && (x.CorreoPromocionInUsers
                                    .Where(y =>
                                            y.IdUsuario == x.IdUsuario
                                            && y.IdCorreoPromocion == idCorreoPromotion
                                            && y.IdEstatusEmail == 1
                                     )
                                    .FirstOrDefault() != null
                                    ||
                                    x.CorreoPromocionInUsers
                                    .Where(y =>
                                            y.IdUsuario == x.IdUsuario
                                            && y.IdCorreoPromocion == idCorreoPromotion
                                     )
                                    .FirstOrDefault() == null)
                                ))
                        .AsNoTracking()
                        .Take(300)
                        .Include(x => x.IdPersonaNavigation)
                        .ToListAsync();

                    return users;
                }
            }
            catch (Exception)
            {
                return new();
            }
        }
        private async Task<int> UserStatusToSendEmail(int idUser, int idCorreoPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var usrFinder = await _Context.CorreoPromocionInUsers.Where(x =>
                        //DateTime.Compare(x.FechaSender.AddDays(1), DateTime.Now) <= 0
                        //&& x.IdEstatusEmail == 3
                        x.IdCorreoPromocion == idCorreoPromotion
                        && x.IdUsuario == idUser
                    )
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    if (usrFinder == null || DateTime.Compare(usrFinder.FechaSender.AddDays(1), DateTime.Now) <= 0)
                        return 1;

                    if (DateTime.Compare(usrFinder.FechaSender.AddDays(1), DateTime.Now) > 0)
                        return 5;
                }
            }
            catch (Exception err)
            {
            }
            return 4;
        }
        public async Task<List<CorreoPromocionInUser>> GetUserStatusByIdEmail(int idCorreoPromotion)
        {
            try
            {
                return await Context.CorreoPromocionInUsers
                .Where(x => x.IdCorreoPromocionNavigation.IdCorreo == idCorreoPromotion)
                .Include(x => x.IdEstatusEmailNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .ThenInclude(x => x.IdPersonaNavigation)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Correo según id de la promoción.");
            }
        }
        public async Task<List<CorreoPromocionInUser>> GetUsersByIdEmail(int idPromotion)
        {
            try
            {
                return await Context.CorreoPromocionInUsers
                .Where(x => x.IdCorreoPromocionNavigation.IdPromocion == idPromotion)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Usuarios por promoción.");
            }
        }
        public async Task<Boolean> isSendingEmail(int idPromotion)
        {
            try
            {
                var emailPromotion = await Context.CorreoPromocions
                .Where(x => x.IdPromocion == idPromotion)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                if (emailPromotion == null || emailPromotion?.IsSendingEmails == 0)
                    return false;

                if (emailPromotion?.IsSendingEmails == 1)
                    return true;

            }
            catch (Exception err)
            {
            }
            return false;
        }
        public async Task<string> setSendingEmail(int idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var emailPromotion = await _Context.CorreoPromocions
                    .Where(x => x.IdPromocion == idPromotion)
                    .FirstOrDefaultAsync();
                    emailPromotion.IsSendingEmails = 1;
                    await _Context.SaveChangesAsync();
                }

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Estado de envío guardado correctamente.");
            }
            return "Proceso realizado correctamente";
        }
        public async Task<string> setCancelSendingEmail(int idPromotion)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var emailPromotion = await _Context.CorreoPromocions
                    .Where(x => x.IdPromocion == idPromotion)
                    .FirstOrDefaultAsync();

                    if(emailPromotion != null)
                    {
                        emailPromotion.IsSendingEmails = 0;
                        await _Context.SaveChangesAsync();
                    }

                    await _Context.Database
                        .ExecuteSqlRawAsync("call sp_setCancel_in_email()");
                }

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Estado de envío no se canceló correctamente.");
            }
            return "Proceso realizado correctamente";
        }
        public async Task<string> setRetrySendPendingEmail(int idCorreoPromotion)
        {
            try
            {
                    using (var _Context = new DBContext())
                    {
                    //var option = _Context.Opcions
                    //.FromSqlRaw("call sp_option_by_endpoint_method_rol ({0}, {1}, {2})", idRol, endPoint, method)
                    //.AsEnumerable().Select(x => x.IdOpcion).ToList();

                    await _Context.Database
                        .ExecuteSqlRawAsync("call sp_setPending_in_retry ({0})", idCorreoPromotion);
                    }

            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Ha ocurrido un error al cambiar el estado del envío.");
            }
            return "Proceso realizado correctamente";
        }
        public async Task<string> setStatusEmailSender(PromoSendgridResponseEntity emailSenderStatus)
        {
            try
            {
                using (var _Context = new DBContext())
                {
                    var json = JsonSerializer.Serialize(emailSenderStatus);
                    await _Context.Database
                        .ExecuteSqlRawAsync("call sp_setStatus_emailSender ({0})", json);
                }
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Ha ocurrido un error al cambiar el estado del envío.");
            }
            return "Proceso realizado correctamente";
        }
    }
}

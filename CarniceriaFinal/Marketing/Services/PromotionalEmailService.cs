using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.Email.Services.IServices;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Interfaces.IRepository;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Services
{
    public class PromotionalEmailService : IPromotionalEmailService
    {
        private readonly IPromotionalEmailRepository IPromotionalEmailRepo;
        private readonly IEmailService emailServi;
        private readonly IPromotionRepository IPromotionRepo;
        private readonly IMapper IMapper;
        public PromotionalEmailService(IPromotionalEmailRepository IPromotionalEmailRepository, 
            IMapper IMapper, IPromotionRepository IPromotionRepository,
            IEmailService IEmailService)
        {
            IPromotionalEmailRepo = IPromotionalEmailRepository;
            IPromotionRepo = IPromotionRepository;
            this.IMapper = IMapper;
            emailServi = IEmailService;
        }
        public async Task<EmailResponseEntity> GetEmailByidPromotion(int idPromotion)
        {
            try
            {
                var promActivate = await IPromotionRepo.PromotionIsActivate(idPromotion);

                var promo = await IPromotionalEmailRepo.GetEmailByidPromotion(idPromotion);
                EmailResponseEntity promoResponse = new();
                promoResponse.enableSendEmail = true;

                var hasInbox = await IPromotionalEmailRepo.GetUsersByIdEmail(idPromotion);
                promoResponse.hasInbox = false;

                if (hasInbox.Count > 0)
                {
                    promoResponse.hasInbox = true;
                    promoResponse.enableSendEmail = false;
                }
                
                promoResponse.emailPromotion = null;

                if (promo != null)
                {
                    promoResponse.emailPromotion = IMapper.Map<EmailEntity>(promo);
                }

                if (promActivate == null || promo == null)
                {
                    promoResponse.enableSendEmail = false;
                }

                return promoResponse;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la promoción.");
            }

        }
        public async Task<EmailSenderStatusEntity> CreateEmailByIdPromotion(EmailCreateEntity emailPromotion)
        {
            try
            {
                var promo = await IPromotionalEmailRepo.CreateEmailByIdPromotion(
                        IMapper.Map<CorreoPromocion>(emailPromotion)
                    );
                EmailSenderStatusEntity sendEmail = new();
                var promActivate = await IPromotionRepo.PromotionIsActivate(emailPromotion.idPromocion);
                sendEmail.enableSendEmail = true;
                sendEmail.hasInbox = false;

                if (promActivate == null)
                    sendEmail.enableSendEmail = false;

                return sendEmail;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al crear la promoción.");
            }

        }
        public async Task<EmailResponseEntity> UpdateEmailByidEmail(EmailCreateEntity emailPromotion)
        {
            try
            {
                var promo = await IPromotionalEmailRepo.UpdateEmailByidEmail(
                        IMapper.Map<CorreoPromocion>(emailPromotion)
                    );
                EmailResponseEntity sendEmail = new();
                var promActivate = await IPromotionRepo.PromotionIsActivate(emailPromotion.idPromocion);
                sendEmail.enableSendEmail = true;
                sendEmail.emailPromotion = IMapper.Map<EmailEntity>(promo);

                if (promActivate == null)
                    sendEmail.enableSendEmail = false;

                return sendEmail;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar la promoción.");
            }

        }
        public async Task<string> SendPromotionalEmail(int idPromotion, int idCorreoPromotion)
        {
            try
            {
                var promotion = await IPromotionRepo.PromotionIsActivate(idPromotion);
                if (promotion == null)
                    throw RSException.BadRequest("La promoción no se encuentra activa.");

                await IPromotionalEmailRepo.setSendingEmail(idPromotion);

                try
                {
                    List<Usuario> users = null;
                    do
                    {
                        PromoSendgridResponseEntity sendResponse = new();
                        if (!(await IPromotionalEmailRepo.isSendingEmail(idPromotion)))
                            break;

                        users = await IPromotionalEmailRepo.GetUsersToSendEmail(idCorreoPromotion);
                        if (users == null) break;
                        if (users.Count == 0) continue;

                        List<UserToSendPromoEmailEntity> usersToEmail = new();
                        foreach (var user in users)
                        {
                            usersToEmail.Add(new()
                            {
                                email = user.IdPersonaNavigation.Email,
                                idEmail = idCorreoPromotion,
                                idUser = user.IdUsuario,
                                nameUser = user.Username
                            });
                        }
                        
                        sendResponse.sent = new();
                        sendResponse.wrong = new();

                        await emailServi.SendPromoEmailRequest(usersToEmail, sendResponse);

                        await IPromotionalEmailRepo.setStatusEmailSender(sendResponse);
                    } while (users != null);
                }
                catch (Exception)
                {
                }
                await IPromotionalEmailRepo.setCancelSendingEmail(idPromotion);
                return "Correo enviado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al enviar la promoción.");
            }
        }
        public async Task<string> RetrySenderEmailByIdPromotion(int idPromotion, int idCorreoPromotion)
        {
            try
            {
                var promotion = await IPromotionRepo.PromotionIsActivate(idPromotion);
                if (promotion == null)
                    throw RSException.BadRequest("La promoción no se encuentra activa.");

                await IPromotionalEmailRepo.setSendingEmail(idPromotion);
                await IPromotionalEmailRepo.setRetrySendPendingEmail(idCorreoPromotion);

                List<Usuario> users = null;
                do
                {
                    PromoSendgridResponseEntity sendResponse = new();
                    if (!(await IPromotionalEmailRepo.isSendingEmail(idPromotion)))
                        break;

                    users = await IPromotionalEmailRepo.GetUsersToRetryEmailByIdPromotion(idCorreoPromotion);
                    if (users == null) break;
                    if (users.Count == 0) continue;

                    List<UserToSendPromoEmailEntity> usersToEmail = new();
                    foreach (var user in users)
                    {
                        usersToEmail.Add(new()
                        {
                            email = user.IdPersonaNavigation.Email,
                            idEmail = idCorreoPromotion,
                            idUser = user.IdUsuario,
                            nameUser = user.Username
                        });
                    }

                    sendResponse.sent = new();
                    sendResponse.wrong = new();


                    await emailServi.SendPromoEmailRequest(usersToEmail, sendResponse);
                    await IPromotionalEmailRepo.setStatusEmailSender(sendResponse);
                } while (users != null);

            }
            catch (Exception err)
            {
            }
            await IPromotionalEmailRepo.setCancelSendingEmail(idPromotion);
            return "Realizado correctamente";
        }
        public async Task<string> CancelSenderEmailByIdPromotion(int idPromotion)
        {
            try
            {
                await IPromotionalEmailRepo.setCancelSendingEmail(idPromotion);
                return "Proceso realizado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al cancelar la promoción.");
            }

        }
        public async Task<UserStatusEmailEntity> GetUserStatusByIdEmail(int idCorreoPromotion)
        {
            UserStatusEmailEntity response = new();
            List<UserPromotionalEmailEntity> promotionalUsers = new();
            try
            {
                var users = await IPromotionalEmailRepo.GetUserStatusByIdEmail(idCorreoPromotion);
                if (users == null || users.Count == 0)
                    return response;

                foreach (var user in users)
                {
                    try
                    {
                        promotionalUsers.Add(new()
                        {
                            email = user.IdUsuarioNavigation.IdPersonaNavigation.Email != null
                                ? user.IdUsuarioNavigation.IdPersonaNavigation.Email : "",
                            statusSender = user.IdEstatusEmailNavigation.Titulo,
                            usuario = user.IdUsuarioNavigation.Username != null
                                ? user.IdUsuarioNavigation.Username : "",
                            enableToSend = DateTime.Compare(user.FechaSender, DateTime.Now) < 0
                                            ? user.FechaSender.AddDays(1)
                                            : DateTime.Now
                        });
                    }
                    catch (Exception err)
                    {
                    }
                }
                response.users = promotionalUsers;
                response.isAvailabilityResend  = await IPromotionalEmailRepo
                    .isAvailabilityResend(idCorreoPromotion);
            }
            catch (Exception)
            {
            }
            return response;
        }

    }
}

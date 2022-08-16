using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Repository.IRepository
{
    public interface IPromotionalEmailRepository
    {
        Task<CorreoPromocion> GetEmailByidPromotion(int idPromotion);
        Task<CorreoPromocion> CreateEmailByIdPromotion(CorreoPromocion email);
        Task<CorreoPromocion> UpdateEmailByidEmail(CorreoPromocion email);
        Task<string> CancelSenderEmailByIdPromotion(int idPromotion);
        Task<List<Usuario>> GetUsersToRetryEmailByIdPromotion(int idCorreoPromotion);
        Task<List<Usuario>> GetUsersAvailablesToSendEmail(int idCorreoPromotion);
        Task<List<Usuario>> GetUsersToSendEmail(int idCorreoPromotion);
        Task<List<CorreoPromocionInUser>> GetUserStatusByIdEmail(int idEmail);
        Task<List<CorreoPromocionInUser>> GetUsersByIdEmail(int idPromotion);
        Task<Boolean> isSendingEmail(int idPromotion);
        Task<string> setSendingEmail(int idPromotion);
        Task<string> setCancelSendingEmail(int idPromotion);
        Task<string> setRetrySendPendingEmail(int idCorreoPromotion);
        Task<string> setStatusEmailSender(PromoSendgridResponseEntity emailSenderStatus);
        Task<Boolean> isAvailabilityResend(int idCorreoPromocion);
    }
}

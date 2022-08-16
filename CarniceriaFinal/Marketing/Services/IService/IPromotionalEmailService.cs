using CarniceriaFinal.Marketing.DTOs;

namespace CarniceriaFinal.Marketing.Services.IService
{
    public interface IPromotionalEmailService
    {
        Task<EmailResponseEntity> GetEmailByidPromotion(int idPromotion);
        Task<EmailSenderStatusEntity> CreateEmailByIdPromotion(EmailCreateEntity emailPromotion);
        Task<EmailResponseEntity> UpdateEmailByidEmail(EmailCreateEntity emailPromotion);
        Task<string> SendPromotionalEmail(int idPromotion, int idCorreoPromotion);
        Task<string> RetrySenderEmailByIdPromotion(int idPromotion, int idCorreoPromotion);
        Task<string> CancelSenderEmailByIdPromotion(int idPromotion);
        Task<UserStatusEmailEntity> GetUserStatusByIdEmail(int idCorreoPromotion);
    }
}

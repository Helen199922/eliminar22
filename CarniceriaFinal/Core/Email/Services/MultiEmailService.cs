using CarniceriaFinal.Core.Email.Services.IServices;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.ModelsEF;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CarniceriaFinal.Core.Email.Services
{
    public class MultiEmailService : IMultiEmailService
    {
        private readonly DTOs.MailSettings _mailOptions;
        private readonly IConfiguration configuration;
        private Boolean IS_CANCEL_PROMO = false;
        private Boolean IS_SENDING_PROMO = false;
        private IServiceScopeFactory Services { get; }

        public MultiEmailService(IOptions<DTOs.MailSettings> mailOptions, IConfiguration _configuration, 
            IServiceScopeFactory services)
        {

            configuration = _configuration;
            _mailOptions = mailOptions.Value;
            Services = services;
        }
        //public async Task SendPromoEmailRequest(List<UserToSendPromoEmailEntity> usersToEmail, PromoSendgridResponseEntity sendResponse)
        //{
        //    using var scope = Services.CreateScope();
        //    var emailServi = scope.ServiceProvider.GetRequiredService<IEmailService>();
        //    var IPromotionalEmailRepo = scope.ServiceProvider.GetRequiredService<IPromotionalEmailRepository>();

        //    var value = "";
        //    try
        //    {
        //        string accounts = "";
        //        var Placeholders = new List<KeyValuePair<string, string>>();

        //        var client = new SendGridClient(_mailOptions.Password);
        //        var tasks = new List<Task<UserToSendPromoEmailEntity>>();

        //        foreach (var user in usersToEmail)
        //        {
        //            var message = new SendGridMessage
        //            {
        //                From = new EmailAddress(_mailOptions.Mail, _mailOptions.DisplayName),
        //                Subject = "Compra de Carne - El Zamorano",
        //                HtmlContent = emailServi.UpdatePlaceHolders(emailServi.GetEmailBody("products-request"), Placeholders),
        //            };

        //            message.AddTo(new EmailAddress(user.email, user.nameUser));

        //            tasks.Add(Task.Run(async () =>
        //            {
        //                user.statusSenderGrid = await client.SendEmailAsync(message);

        //                return user;
        //            }));
        //            //var response = await client.SendEmailAsync(message);
        //            //response.IsSuccessStatusCode

        //        };


        //        var resp = Task.WhenAll(tasks);
        //        await resp;

        //        foreach (var response in resp.Result)
        //        {
        //            if (response.statusSenderGrid.IsSuccessStatusCode) sendResponse.sent.Add(new()
        //            {
        //                idPromotionalEmail = response.idEmail,
        //                idUser = response.idUser
        //            });
        //            else sendResponse.wrong.Add(new()
        //            {
        //                idPromotionalEmail = response.idEmail,
        //                idUser = response.idUser
        //            });
        //        }


        //    }
        //    catch (Exception err)
        //    {
        //        //throw new RSException(err.TypeError, err.Code, err.MessagesError);
        //    }
        //}

        //public async void SendPromotionalEmail(int idCorreoPromotion)
        //{
        //    IS_SENDING_PROMO = true;
        //    IS_CANCEL_PROMO = false;
        //    using var scope = Services.CreateScope();
        //    var emailServi = scope.ServiceProvider.GetRequiredService<IEmailService>();
        //    var IPromotionalEmailRepo = scope.ServiceProvider.GetRequiredService<IPromotionalEmailRepository>();


        //    try
        //    {
        //        PromoSendgridResponseEntity sendResponse = new();
        //        List<Usuario> users = new();
        //        do
        //        {
        //            Console.WriteLine("Holaaaaaaaaaaaaaaaaaa");
        //            if (this.IS_CANCEL_PROMO) break;

        //            users = await IPromotionalEmailRepo.GetUsersToSendEmail(idCorreoPromotion);
        //            if (users.Count == 0) break;

        //            List<UserToSendPromoEmailEntity> usersToEmail = new();
        //            foreach (var user in users)
        //            {
        //                usersToEmail.Add(new()
        //                {
        //                    email = user.IdPersonaNavigation.Email,
        //                    idEmail = idCorreoPromotion,
        //                    idUser = user.IdUsuario,
        //                    nameUser = user.Username
        //                });
        //            }

        //            sendResponse.sent = new();
        //            sendResponse.wrong = new();

        //            await SendPromoEmailRequest(usersToEmail, sendResponse);
        //        } while (users.Count > 0);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    IS_SENDING_PROMO = false;
        //}
    }
}

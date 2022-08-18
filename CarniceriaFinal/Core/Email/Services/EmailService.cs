using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Core.Email.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using System.IO;
using CarniceriaFinal.Core.CustomException;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using CarniceriaFinal.Marketing.DTOs;

namespace CarniceriaFinal.Core.Email.Services
{
    public class KeyVaultModel
    {
        public string? ConnectionStringKeyVault { get; set; }
    }
    public class EmailService : IEmailService
    {

        private const string templatePath = @"Core/Email/Templates/{0}.html";
        private readonly DTOs.MailSettings _mailOptions;
        private readonly IConfiguration configuration;
        public EmailService(IOptions<DTOs.MailSettings> mailOptions, IConfiguration _configuration)
        {
            configuration = _configuration;
            _mailOptions = mailOptions.Value;
        }
        public async Task<string> SendEmailAsync(EmailRequest mailRequest)
        {
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _mailOptions.DisplayName,
                _mailOptions.Mail
            ));
            message.To.Add(new MailboxAddress(
                _mailOptions.DisplayName,
                mailRequest.ToEmail
            ));
            message.Subject = mailRequest.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                userName: "apikey",
                password: _mailOptions.Password
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return "Correo enviado correctamente";
        }
        
        public async Task<string> SendEmailToProductRequest(EmailProductsRequest mailRequest)
        {
            var value = "";
            try
            {
                string accounts = "";
                foreach (var account in mailRequest.accounts)
                {
                    accounts = accounts + ("<tr>" +
                            "<td style='border: 1px dotted black;color: #002F5E;padding:15px;width:100px;'> " + account.bankName + "</td>" +
                            "<td style='border: 1px dotted black;color: #002F5E;padding:15px;width:100px;'>" + account.typeAccount + "</td>" +
                            "<td style='border: 1px dotted black;color: #002F5E;padding:15px;width:100px;'>" + account.numAccount + "</td>" +
                        "</tr>");
                }
                string productsDetail = "";
                foreach (var detail in mailRequest.productDetail)
                {
                    productsDetail = productsDetail + ("<p style='color: #000;'>" +
                        "<strong>" + detail.product + " (" + detail.quantity + "): </strong>"+ detail.amount +
                        " = $"+ detail.finalAmount +
                        
                        "</p><p> " + detail.discount + "</p>");

                }
                var Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{userName}}", mailRequest.userName),
                    new KeyValuePair<string, string>("{{email}}", mailRequest.email),
                    new KeyValuePair<string, string>("{{amount}}", mailRequest.amount.ToString()),
                    new KeyValuePair<string, string>("{{bankDetail}}", accounts),
                    new KeyValuePair<string, string>("{{productDetail}}", productsDetail),
                    new KeyValuePair<string, string>("{{numPedido}}", mailRequest.numPedido),

                    new KeyValuePair<string, string>("{{transporte}}", mailRequest.transporte),
                    new KeyValuePair<string, string>("{{descuento}}", mailRequest.discount),
                    new KeyValuePair<string, string>("{{subTotal}}", mailRequest.subTotal),
                    new KeyValuePair<string, string>("{{total}}", Math.Round((decimal)mailRequest.amount, 2).ToString())
                };
                EmailRequest emailData = new()
                {
                    Body = this.UpdatePlaceHolders(this.GetEmailBody("products-request"), Placeholders),
                    Subject = "Compra de Carne - El Zamorano",
                    ToEmail = mailRequest.email
                };
                await this.SendEmailAsync(emailData);
            }
            catch (Exception err)
            {
                //throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            return value;
        }

        public async Task SendPromoEmailRequest(List<UserToSendPromoEmailEntity> usersToEmail, PromoSendgridResponseEntity sendResponse, EmailEntity promo)
        {
            var value = "";
            try
            {
                string accounts = "";
                var Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{headerImage}}", promo.imagen),
                    new KeyValuePair<string, string>("{{titulo}}", promo.titulo),
                    new KeyValuePair<string, string>("{{descripcion}}", promo.descripcion),
                    new KeyValuePair<string, string>("{{productImage}}", promo.imageProducts)
                };

                var client = new SendGridClient(_mailOptions.Password);
                var tasks = new List<Task<UserToSendPromoEmailEntity>>();

                    foreach (var user in usersToEmail)
                    {
                        var message = new SendGridMessage
                        {
                            From = new EmailAddress(_mailOptions.Mail, _mailOptions.DisplayName),
                            Subject = promo.titulo,
                            HtmlContent = this.UpdatePlaceHolders(this.GetEmailBody("email-promotion"), Placeholders),
                        };

                        message.AddTo(new EmailAddress(user.email, user.nameUser));

                        tasks.Add(Task.Run( async () =>
                        {
                            user.statusSenderGrid = await client.SendEmailAsync(message);

                            return user;
                        }));
                    };


                    var resp = Task.WhenAll(tasks);
                    await resp;

                    foreach (var response in resp.Result)
                    {
                        if (response.statusSenderGrid.IsSuccessStatusCode) sendResponse.sent.Add(new()
                        {
                            idPromotionalEmail = response.idEmail,
                            idUser = response.idUser
                        });
                        else sendResponse.wrong.Add(new()
                        {
                            idPromotionalEmail = response.idEmail,
                            idUser = response.idUser
                        });
                }


            }
            catch (Exception err)
            {
                //throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
        }
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if(!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }
    }
}

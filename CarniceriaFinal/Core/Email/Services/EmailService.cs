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

namespace CarniceriaFinal.Core.Email.Services
{
    public class KeyVaultModel
    {
        public string? ConnectionStringKeyVault { get; set; }
    }
    public class EmailService : IEmailService
    {
        private const string templatePath = @"Core/Email/Templates/{0}.html";
        private readonly IConfiguration _Configuration;
        public EmailService(IConfiguration IConfiguration)
        {
            this._Configuration = IConfiguration;
        }
        public async Task<string> SendEmailAsync(EmailRequest mailRequest)
        {
            //var client = new SendGridClient(_Configuration["sendgrid-test"]);

            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                "Carnicería Zamorano",
                "jimy.coxr@ug.edu.ec"
            ));
            message.To.Add(new MailboxAddress(
                "Carnicería Zamorano",
                mailRequest.ToEmail
            ));
            message.Subject = mailRequest.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = mailRequest.Body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                userName: "apikey",
                password: _Configuration["SendGrid:ClientSecret"]
            );

            Console.WriteLine("Sending email");
            await client.SendAsync(message);
            Console.WriteLine("Email sent");

            await client.DisconnectAsync(true);

            //var from = new EmailAddress("jimy.coxr@ug.edu.ec", mailRequest.Subject);
            //var to = new EmailAddress(mailRequest.ToEmail);
            //var msg = MailHelper.CreateSingleEmail(from, to, mailRequest.Subject, "", mailRequest.Body);
            //var email = await client.SendEmailAsync(msg);

            return "Correo enviado correctamente";
        }
        
        public async Task<string> SendEmailToProductRequest(EmailProductsRequest mailRequest)
        {
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
                        
                        "</p>");

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
            return "Email Enviado";
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

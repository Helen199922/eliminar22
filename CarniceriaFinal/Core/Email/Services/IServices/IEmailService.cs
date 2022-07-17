using CarniceriaFinal.Core.Email.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.Email.Services.IServices
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(EmailRequest mailRequest);
        Task<string> SendEmailToProductRequest(EmailProductsRequest mailRequest);
    }
}

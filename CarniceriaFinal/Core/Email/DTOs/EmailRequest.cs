using CarniceriaFinal.Marketing.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.Email.DTOs
{
    public class MailSettings
    {
        public string? Mail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return String.Concat(Mail," || " , DisplayName, " || ", Password, " || ", Host, " || ", Port);
        }

    }
    public class EmailRequest
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
    public class AccountsEntity
    {
        public string? bankName { get; set; }
        public string? typeAccount { get; set; }
        public string? numAccount { get; set; }
    }
    public class DetailProductsEntity
    {
        public string? product { get; set; }
        public string? quantity { get; set; }
        public string? amount { get; set; }
        public string? finalAmount { get; set; }
        public string? discount { get; set; }
        public string? typeDiscount { get; set; }
    }
    public class EmailProductsRequest
    {
        public string? userName { get; set; }
        public string? numPedido { get; set; }
        public string? email { get; set; }
        public float amount { get; set; }
        public string? discount { get; set; }
        public string? subTotal { get; set; }
        public string? transporte { get; set; }
        public List<AccountsEntity> accounts { get; set; }
        public List<DetailProductsEntity> productDetail { get; set; }
    }
}

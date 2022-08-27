using CarniceriaFinal.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Models
{
    public class SaleNoUserRequestEntity
    {
        public string? email { get; set; }
        public string? nombreVenta { get; set; }
        public string? cedula { get; set; }
        public List<SaleDetailEntity> detalleVenta { get; set; }
        //public ClientEntity cliente { get; set; }
    }

    public class SalesUserInformationResponse {
        public int numVenta { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
    }
}

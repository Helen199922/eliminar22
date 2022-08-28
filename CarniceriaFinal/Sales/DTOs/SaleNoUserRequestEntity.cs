using CarniceriaFinal.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Models
{
    public class SaleNoUserRequestEntity
    {
        public List<SaleDetailEntity> detalleVenta { get; set; }
        public ClientdetailRquest cliente { get; set; }
        public int isAuthUser { get; set; }
    }

    public class ClientdetailRquest
    {
        public string? email { get; set; }
        public string? apellido { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? cedula { get; set; }
    }
    public class SalesUserInformationResponse {
        public int numVenta { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
    }
}

using CarniceriaFinal.Cliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Models
{
    public class SaleNoUserRequestEntity : SaleEntity
    {
        public List<SaleDetailEntity> detalleVenta { get; set; }
        public ClientEntity cliente { get; set; }
    }
}

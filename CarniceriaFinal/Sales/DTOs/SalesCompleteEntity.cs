using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.DTOs
{
    public class SalesCompleteEntity
    {
        public SaleEntity venta { get; set; }
        public List<SaleDetailEntity> detalleVenta { get; set; }
        public SalesCompleteEntity(SaleEntity sale, List<SaleDetailEntity> details)
        {
            this.venta = sale;
            this.detalleVenta = details;
        }
    }
}

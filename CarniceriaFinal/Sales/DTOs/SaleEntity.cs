using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Models
{
    public class SaleEntity
    {
        public SaleEntity(int? idVenta, int? idCliente, DateTime? fecha, int idImpuesto, float total, int status, float costosAdicionales, string motivoCostosAdicional, int idFormaPago, string direccion, string referencia, int idCiudad)
        {
            this.idVenta = idVenta;
            this.idCliente = idCliente;
            this.fecha = fecha;
            this.idImpuesto = idImpuesto;
            this.total = total;
            this.status = status;
            this.costosAdicionales = costosAdicionales;
            this.motivoCostosAdicional = motivoCostosAdicional;
            this.idFormaPago = idFormaPago;
            this.direccion = direccion;
            this.referencia = referencia;
            this.idCiudad = idCiudad;
        }

        public SaleEntity(){}

        public int? idVenta { get; set; }
        public int? idCliente {get; set;}
        public DateTime? fecha {get; set;}
        public int idImpuesto {get; set;}
        public float total {get; set;}
        public int status {get; set;}
        public float costosAdicionales {get; set;}
        public string? motivoCostosAdicional {get; set;}
        public int idFormaPago {get; set;}
        public string? direccion { get; set; }
        public string? referencia { get; set; }
        public int idCiudad { get; set; }
    }
}

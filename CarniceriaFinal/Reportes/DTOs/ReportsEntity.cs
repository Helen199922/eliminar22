using CarniceriaFinal.Core.JWTOKEN.DTOs;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Sales.Models;

namespace CarniceriaFinal.Reportes.DTOs
{
    public class ReportResponse<T>
    {
        public int totalData { get; set; }
        public T dataReport { get; set; }
    }
    public class MiltiFieldReportEntity
    {
        public string name { get; set; }
        public List<FieldReportEntity> series { get; set; }
    }
    public class FieldReportEntity
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    //SaleDetailEntity
    public class FieldReportAmountEntity
    {
        public string name { get; set; }
        public float value { get; set; }
    }

    public class ReportByDatesAndCategory : ReportByDates
    {
        public int idCategory { get; set; }
    }

    public class ReportGraficsByDatesAndModules : ReportByDates
    {
        public List<int> idModules { get; set; }
    }

    public class ReportByDates
    {
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
    }
    public class CategoriesReports
    {
        public string value { get; set; }
        public int id { get; set; }
    }
    public class ModulesReports
    {
        public string value { get; set; }
        public int id { get; set; }
    }

    public class DataSalesReportDetail
    {
        public List<SalesReportDetail> pendiente { get; set; }
        public List<SalesReportDetail> atendido { get; set; }
        public List<SalesReportDetail> rechazado { get; set; }
    }

    public class SalesReportDetail
    {
        public int? idVenta { get; set; }
        public string cedula { get; set; }
        public DateTime? fecha { get; set; }
        public float total { get; set; }
        public int status { get; set; }
        public float costosAdicionales { get; set; }
        public string? motivoCostosAdicional { get; set; }
        public string formaPago { get; set; }
        public string? direccion { get; set; }
        public string? referencia { get; set; }
        public string ciudad { get; set; }
    }
    public class ProductReportDetail
    {
        public int? idProducto { get; set; }
        public string titulo { get; set; }
        public int ventasAtendidas { get; set; }
        public int ventasRechazadas { get; set; }
        public int ventasPendientes { get; set; }
        public int stock { get; set; }
        public float precio { get; set; }
    }
    public class ModulesReportDetail
    {
        public List<LogsEntity> correcta { get; set; }
        public List<LogsEntity> error { get; set; }
    }

    public class MembershipUserDetailEntity
    {
        public int IdUsuario { get; set; }
        public string usuario { get; set; }
        public string membresiaTitulo { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public int CantProductosComprados { get; set; }

    }

    public class SaleDetailAdmReportEntity
    {
        public string cedulaCliente { get; set; }
        public string usuarioAdministrador { get; set; }
        public float montoTotal { get; set; }
        public DateTime FechaAceptacionVenta { get; set; }
        public string detalleVenta { get; set; }

    }
}

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

    public class ReportByDatesAndCategory : ReportByDates
    {
        public int idCategory { get; set; }
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
}

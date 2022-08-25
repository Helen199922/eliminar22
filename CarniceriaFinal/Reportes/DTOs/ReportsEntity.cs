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
}

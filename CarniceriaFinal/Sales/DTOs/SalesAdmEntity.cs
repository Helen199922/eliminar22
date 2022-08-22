namespace CarniceriaFinal.Sales.DTOs
{
    public class SalesAdmEntity
    {
        public int? idVenta { get; set; }
        public string cedula { get; set; }
        public DateTime? fecha { get; set; }
        public float total { get; set; }
        public int status { get; set; }
        public float? costosAdicionales { get; set; }
        public string? motivoCostosAdicional { get; set; }
        public int idFormaPago { get; set; }
        public string formaPago { get; set; }
        public Boolean hasDiscount { get; set; }
    }
    public class DetailDiscountEntity
    {
        public string titulo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime? ultimaActualizacion { get; set; }
        public int status { get; set; }
    }
    public class ResumeSaleDetail
    {
        public ResumeSaleDetail()
        {
            this.inconsistencias = new();
        }
        public string razonBloqueo { get; set; }
        public DateTime fechaRealizacion { get; set; }
        public DateTime fechaActual { get; set; }
        public StatusSale actuallyStatus { get; set; }
        public List<StatusSale> listStatus { get; set; }
        public List<Inconsistency> inconsistencias { get; set; }
        public List<DetailSaleAdmEntity> details { get; set; }
    }
    public class StatusSale
    {
        public string titulo { get; set; }
        public int id { get; set; }
    }
    public class DetailSaleAdmEntity
    {
        public int idDetalle { get; set; }
        public int idProduct { get; set; }
        public int cantidad { get; set; }
        public int stockActual { get; set; }
        public float precio { get; set; }//valor original del producto
        public int? idPromocion { get; set; }
        public int? idMembresiaInUser { get; set; }
        public float descuento { get; set; }
        public string titulo { get; set; }
        public DetailDiscountEntity detailDiscount { get; set; }
        
    }
    public class Inconsistency
    {
        public string tipoInconsistencia { get; set; }
        public string descripcion { get; set; }
    }
    public class SaleAdmRequestIdSale
    {
        public int idSale { get; set; }
    }
}
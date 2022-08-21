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
        public DateTime ultimaActualizacion { get; set; }
        public DateTime status { get; set; }
    }
    public class DetailSaleAdmEntity
    {
        public int idDetalle { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }//valor original del producto
        public int? idPromocion { get; set; }
        public int? idMembresia { get; set; }
        public float descuento { get; set; }
        public string titulo { get; set; }
        public DetailDiscountEntity detailDiscount { get; set; }
    }
}
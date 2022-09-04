namespace CarniceriaFinal.Marketing.DTOs
{
    public class MemberShipCommEntity
    {
        public MemberCommEntity? Bronce { get; set; }
        public MemberCommEntity? Oro { get; set; }
        public MemberCommEntity? Plata { get; set; }
    }


    public class MemberCommEntity
    {
        public string? memberName { get; set; }
        public string? titleMembership { get; set; }
        public string? imageMembership { get; set; }
    }
    public class MembershipEntity
    {
        public int IdMembresia { get; set; }
        public float PorcentajeDescuento { get; set; }
        public float? MontoMaxAcceso { get; set; }
        public float MontoMinAcceso { get; set; }
        public float MontoMaxDescPorProducto { get; set; }
        public int DiasParaRenovar { get; set; }
        public string Titulo { get; set; } = null!;
        public string Imagen { get; set; } = null!;
        public int DuracionMembresiaDias { get; set; }
        public int? CantProductosMembresia { get; set; }


    }
    public class MembershipUserEntity
    {
        public int IdMembresiaInUsuario { get; set; }
        public int IdUsuario { get; set; }
        public MembershipEntity membresia { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public int CantProductosComprados { get; set; }

    }
}

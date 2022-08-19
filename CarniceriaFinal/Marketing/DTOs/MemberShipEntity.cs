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
    public class MembershipUserEntity
    {
        public int IdMembresiaInUsuario { get; set; }
        public int IdUsuario { get; set; }
        public int IdMembresia { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public int CantProductosComprados { get; set; }
        public int Status { get; set; }

    }
}

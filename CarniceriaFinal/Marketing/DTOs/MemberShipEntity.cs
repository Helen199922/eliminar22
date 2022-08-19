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
}

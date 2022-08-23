namespace CarniceriaFinal.Security.DTOs
{
    public class ChangeReceivedEmailUserEntity
    {
        public Boolean status { get; set; }
    }
    public class ProfileUserEntity
    {
        public string email { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cedula { get; set; }
        public Boolean recibirEmail { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string profileImage { get; set; }
        public string direccion { get; set; }
    }
    public class ProfileSimpleUSerEntity{
        public string email { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cedula { get; set; }
        public Boolean recibirEmail { get; set; }
    }
}

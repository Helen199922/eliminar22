using SendGrid;

namespace CarniceriaFinal.Marketing.DTOs
{
    public class EmailResponseEntity : EmailSenderStatusEntity
    {
        public EmailEntity? emailPromotion { get; set; }
    }
    public class EmailSenderStatusEntity
    {
        public Boolean hasInbox { get; set; }
        public Boolean enableSendEmail { get; set; }
    }
    public class EmailEntity
    {
        public int idCorreo { get; set; }
        public string imagen { get; set; }
        public string urlPromocion { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime? fechaUpdate { get; set; }
    }
    public class EmailCreateEntity
    {
        public int idPromocion { get; set; }
        public string imagen { get; set; }
        public string urlPromocion { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
    }
    public class UserPromotionalEmailEntity
    {
        public int idCorreoPromocionInUser { get; set; }
        public string usuario { get; set; }
        public string email { get; set; }
        public string statusSender { get; set; }
        public DateTime enableToSend { get; set; }
    }
    public class UserToSendPromoEmailEntity
    {
        public UserToSendPromoEmailEntity()
        {
            dateSender = DateTime.Now;
        }
        public int idEmail { get; set; }
        public int idUser{ get; set; }
        public string nameUser { get; set; }
        public string email { get; set; }
        public DateTime dateSender { get; set; }
        public Response statusSenderGrid { get; set; }
    }
    public class UserPromoSendgridEntity
    {
        public int idUser { get; set; }
        public int idPromotionalEmail { get; set; }
    }
    public class PromoSendgridResponseEntity
    {
        public List<UserPromoSendgridEntity> sent { get; set; }
        public List<UserPromoSendgridEntity> wrong { get; set; }
    }
}

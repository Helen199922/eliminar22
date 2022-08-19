using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Repository.IRepository
{
    public interface IMembershipRepository
    {
        Task<MemberShipCommEntity> GetMembershipHome();
        Task<Boolean> AdministrationMembershipTimes();
        Task<MembresiaInUsuario> GetMembershipByIdUser(int idUser);
        Task<Membresium> GetMembershipDetail(int idMembership);
    }
}

using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Sales.Models;

namespace CarniceriaFinal.Marketing.Services.IService
{
    public interface IMembershipService
    {
        Task<MemberShipCommEntity> GetMembershipHome();
        Task<Boolean> AdministrationMembershipTimes();
        Task<Boolean> isValidMembership(List<SaleDetailEntity> details, int idUser);
        Task<MembershipUserEntity> GetMembershipByIdUser(int idUser);
    }
}

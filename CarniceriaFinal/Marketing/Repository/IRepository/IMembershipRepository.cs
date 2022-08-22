using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.ModelsEF;

namespace CarniceriaFinal.Marketing.Repository.IRepository
{
    public interface IMembershipRepository
    {
        Task<MemberShipCommEntity> GetMembershipHome();
        Task<Boolean> AdministrationMembershipTimes();
        Task<MembresiaInUsuario> GetLastMembershipByIdUser(int idUser);
        Task<MembresiaInUsuario> GetMembershipByIdUser(int idUser);
        Task<Boolean> RegisterMembershipToUser(int idUser, Membresium membership);
        Task<Membresium> GetMembershipDetail(int idMembershipInUser);
        Task<MembresiaInUsuario> GetMembershipByIdClient(int IdClient);
        Task<MembresiaInUsuario> GetMembershipDetailToAdmSales(int idMembershipInUser);
        Task<String> AddProductsToMembershipByIdUser(int idMembresiaInUser, int cantProduct);
        Task<Boolean> AdmMemberTimesByIdUserInMembership(int idMembresiaInUser);
        Task<float> AmountToMembershipInUser(int idUser, int idClient);
        Task<List<Membresium>> GetAllMembershipDetail();
        Task<String> RemoveProductsToMembershipByIdUser(int idMembresiaInUser, int cantProduct);
        Task<float> AmountToMemberUserDeclineSale(int idUser, int idClient);
        Task<Boolean> BlockMembershipToUser(int idMembershipInUser);
    }
}

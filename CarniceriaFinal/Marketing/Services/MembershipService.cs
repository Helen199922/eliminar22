using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.DTOs;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.Sales.Models;

namespace CarniceriaFinal.Marketing.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository IMembershipRepository;
        private readonly IMapper IMapper;
        public MembershipService(IMembershipRepository IMembershipRepository,
            IMapper IMapper)
        {
            this.IMembershipRepository = IMembershipRepository;
            this.IMapper = IMapper;
        }
        public async Task<MemberShipCommEntity> GetMembershipHome()
        {
            try
            {
                var memberShipComm = await IMembershipRepository.GetMembershipHome();

                return memberShipComm;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los miembros.");
            }

        }
        public async Task<MembershipUserEntity> GetMembershipByIdUser(int idUser)
        {
            try
            {
                var response = await IMembershipRepository.GetMembershipByIdUser(idUser);
                if (response == null) return null;

                var userMember = IMapper.Map<MembershipUserEntity>(response);

                userMember.membresia = IMapper.Map<MembershipEntity>(response.IdMembresiaNavigation);
                return userMember;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener la membresia del cliente.");
            }

        }
        public async Task<Boolean> AdministrationMembershipTimes()
        {
            try
            {
                return await IMembershipRepository.AdministrationMembershipTimes();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al administrar miembros.");
            }

        }

        public async Task<Boolean> isValidMembership(List<SaleDetailEntity> details, int idUser)
        {
            try
            {
                if (details.Count == 0)
                {
                    return true;
                }

                
                var memberSales = details.Where(x => x.idMembresia != null).FirstOrDefault();

                if (memberSales == null || memberSales.idMembresia == null)
                    throw RSException.Unauthorized("No existe la promoción o no está activa");


                var membershipConditions = await IMembershipRepository.GetMembershipDetail(memberSales.idMembresia.Value);
                var userMemberDetail = await IMembershipRepository.GetMembershipByIdUser(idUser);

                if(userMemberDetail == null)
                    throw RSException.Unauthorized("No existe el cliente con dicha membresía");

                var numberProducts = userMemberDetail.CantProductosComprados;

                foreach (var detail in details)
                {
                    numberProducts = numberProducts + detail.cantidad;
                }
                if(numberProducts > membershipConditions.CantProductosMembresia)
                    throw RSException.Unauthorized("Cantidad de productos para la membresía es superior al límite");

                return true;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al validar la membresía.");
            }

        }
    }
}

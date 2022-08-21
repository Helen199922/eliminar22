using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Repository.IRepository;
using CarniceriaFinal.Sales.Services.IServices;

namespace CarniceriaFinal.Sales.Services
{
    public class SalesAdmServices: ISalesAdmServices
    {
        private readonly ISalesAdmRepository ISalesAdmRepository;
        private readonly IMapper IMapper;
        public SalesAdmServices(
            IMapper IMapper,
            ISalesAdmRepository ISalesAdmRepository
        )
        {
            this.IMapper = IMapper;
            this.ISalesAdmRepository = ISalesAdmRepository;
        }

        public async Task<List<SalesAdmEntity>> GetAllSales(int idStatus)
        {
            try
            {
                return await ISalesAdmRepository.GetAllSales(idStatus);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el estado de la venta");
            }
        }
    }
}

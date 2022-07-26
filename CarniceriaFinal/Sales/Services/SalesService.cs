using AutoMapper;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Core.Email.Services.IServices;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.Repository;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.IRepository;
using CarniceriaFinal.Sales.IServices;
using CarniceriaFinal.Sales.Models;
using CarniceriaFinal.Sales.Repository.IRepository;
using CarniceriaFinal.Sales.Services.IServices;
using CarniceriaFinal.Security.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Sales.Services
{

    public class SalesService : ISalesService
    {
        private readonly IUserRepository IUserRepository;
        private readonly IPersonRepository IPersonRepository;
        private readonly IClientRepository IClientRepository;
        private readonly ISaleRepository ISaleRepository;
        private readonly IProductoRepository IProductoRepository;
        private readonly ICitiesServices ICitiesServices;
        private readonly IEmailService IEmailService;
        private readonly IBankInfoRepository IBankInfoRepository;
        private readonly IMapper IMapper;
        public SalesService(IUserRepository IUserRepository, IPersonRepository IPersonRepository, 
            IClientRepository IClientRepository, ISaleRepository ISaleRepository, 
            IMapper IMapper, IProductoRepository IProductoRepository,
            ICitiesServices ICitiesServices, IEmailService IEmailService,
            IBankInfoRepository IBankInfoRepository)
        {
            this.IUserRepository = IUserRepository;
            this.IPersonRepository = IPersonRepository;
            this.IClientRepository = IClientRepository;
            this.ISaleRepository = ISaleRepository;
            this.IProductoRepository = IProductoRepository;
            this.ICitiesServices = ICitiesServices;
            this.IEmailService = IEmailService;
            this.IBankInfoRepository = IBankInfoRepository;
            this.IMapper = IMapper;
        }
        public async Task<string> CreateSaleNoUser(SaleNoUserRequestEntity sale)
        {
            try
            {
                sale.referencia = "Costo de Envío";
                sale.costosAdicionales = await ICitiesServices.GetCityCostById(sale.idCiudad);
                float finalAmount = (float)sale.costosAdicionales;
                float subTotal = 0;
                float discount = 0;

                List<DetailProductsEntity> salesDetails = new();
                foreach (var item in sale.detalleVenta)
                {
                    float discountInDetail = 0;
                    var productValue = await this.IProductoRepository.FindProductById(item.idProducto);
                    if (productValue == null) throw RSException.BadRequest("El producto no se encontró");

                    
                    if (item.idPromocion != null)
                    {
                        discountInDetail = await getDiscountTotalByProductAndPromotion(item);
                        if(discountInDetail == 0)
                            item.idPromocion = null;
                    }


                    item.precio = productValue.Precio.Value;
                    item.descuentoTotal = discountInDetail;
                    subTotal += (productValue.Precio.Value * item.cantidad);
                    finalAmount += (productValue.Precio.Value * item.cantidad) - discountInDetail;

                    discount += discountInDetail;


                    salesDetails.Add(new DetailProductsEntity
                    {
                        amount = productValue.Precio.Value.ToString(),
                        product = productValue.Titulo,
                        quantity = item.cantidad.ToString(),
                        finalAmount = Math.Round((productValue.Precio.Value * item.cantidad), 2).ToString()
                    });
                }
                sale.total = finalAmount;

                //Verificar si el cliente no está registrado como un usuario con clave y contraseña
                var user = await IUserRepository.GetUserByIdIndentificationPerson(sale.cliente.cedula);
                if(user != null)
                {
                    throw RSException.Unauthorized("Ya existe un usuario registrado con estas credenciales. Por favor, inicie sesión.");
                }
                var client = await IClientRepository.GetClientByIdentification(sale.cliente.cedula);
                
                //actualizar persona
                int idClient = 0;
                if (client != null)
                {
                    idClient = UpdateClientNoUser(sale.cliente, client).Result.IdCliente;
                    var personUpdated = await UpdatePersonNoUser(sale.cliente, client.IdPersonaNavigation);
                }
                else
                {
                    var person = await CreatePerson(sale.cliente);
                    idClient = CreateClient(sale.cliente, person.IdPersona).Result.IdCliente;
                }

                sale.fecha = DateTime.Now;
                Ventum saleCreated = await CreateSale(sale, idClient);
                
                //Crear datalle
                foreach (var detail in sale.detalleVenta)
                {
                    await CreateDetail(detail, saleCreated.IdVenta);
                }


                var email = await this.IEmailService.SendEmailToProductRequest(new EmailProductsRequest
                {
                    numPedido = saleCreated.IdVenta.ToString(),
                    amount= finalAmount,
                    email=sale.cliente.email,
                    userName=sale.cliente.nombre,
                    accounts= await GetAllBanks(),
                    discount = discount.ToString(),
                    subTotal = Math.Round(subTotal, 2).ToString(),
                    productDetail = salesDetails,
                    transporte = sale.costosAdicionales.ToString()
                });

                return email;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar la venta");
            }
        }
        public async Task<string> CreateSaleUser(SaleNoUserRequestEntity sale)
        {
            try
            {
                sale.referencia = "Costo de Envío";
                sale.costosAdicionales = await ICitiesServices.GetCityCostById(sale.idCiudad);
                float finalAmount = (float)sale.costosAdicionales;
                float subTotal = 0;
                float discount = 0;
                List<DetailProductsEntity> salesDetails = new();
                foreach (var item in sale.detalleVenta)
                {
                    float discountInDetail = 0;
                    var productValue = await this.IProductoRepository.FindProductById(item.idProducto);
                    if (productValue == null) throw RSException.BadRequest("El producto no se encontró");

                    if (item.idPromocion != null)
                    {
                        discountInDetail = await getDiscountTotalByProductAndPromotion(item);
                        if (discountInDetail == 0)
                            item.idPromocion = null;
                    }

                    item.precio = productValue.Precio.Value;
                    item.descuentoTotal = discountInDetail;
                    subTotal += (productValue.Precio.Value * item.cantidad);
                    finalAmount += (productValue.Precio.Value * item.cantidad) - discountInDetail;

                    discount += discountInDetail;

                    salesDetails.Add(new DetailProductsEntity
                    {
                        amount = productValue.Precio.Value.ToString(),
                        product = productValue.Titulo,
                        quantity = item.cantidad.ToString(),
                        finalAmount = Math.Round((productValue.Precio.Value * item.cantidad), 2).ToString()
                    });

                }
                sale.total = finalAmount;

                //Verificar si el cliente está registrado como un usuario con clave y contraseña
                var user = await IUserRepository.GetUserByIdIndentificationPerson(sale.cliente.cedula);
                if (user == null)
                {
                    throw RSException.Unauthorized("Ha ocurrido un error con su cuenta de usuario. Por favor, contactarse con el administrador.");
                }
                var client = await IClientRepository.GetClientByIdentification(sale.cliente.cedula);
                //actualizar persona
                int idClient = 0;
                if (client == null)
                {
                    var clientCreated = await CreateClient(sale.cliente, user.IdPersona.Value);
                    idClient = clientCreated.IdCliente;
                }
                else
                {
                    idClient = client.IdCliente;
                }

                sale.fecha = DateTime.Now;
                Ventum saleCreated = await CreateSale(sale, idClient);
                //Crear datella

                foreach (var detail in sale.detalleVenta)
                {
                    await CreateDetail(detail, saleCreated.IdVenta);
                }

                var email = await this.IEmailService.SendEmailToProductRequest(new EmailProductsRequest
                {
                    numPedido = saleCreated.IdVenta.ToString(),
                    amount = finalAmount,
                    email = sale.cliente.email,
                    userName = sale.cliente.nombre,
                    accounts = await GetAllBanks(),
                    discount = discount.ToString(),
                    subTotal = Math.Round(subTotal, 2).ToString(),
                    productDetail = salesDetails,
                    transporte = sale.costosAdicionales.ToString()
                });

                return email;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar la venta");
            }
        }

        public async Task<ModelsEF.Cliente> CreateClient(ClientEntity clientNewData, int idPerson)
        {
            try
            {
                ModelsEF.Cliente client = new();
                client.Referencia = clientNewData.referencia;
                client.IdCiudad = clientNewData.idCiudad;
                client.Telefono1 = clientNewData.telefono1;
                client.Telefono2 = clientNewData.telefono2;
                client.Direccion = clientNewData.direccion;
                client.IdPersona = idPerson;

                return await IClientRepository.CreateClient(client);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el cliente");
            }
        }

        public async Task<Persona> CreatePerson(ClientEntity clientNewData)
        {
            try
            {
                Persona person = new();
                person.Email = clientNewData.email;
                person.Nombre = clientNewData.nombre;
                person.Apellido = clientNewData.apellido;
                person.IdSexo = clientNewData.idSexo;
                person.Cedula = clientNewData.cedula;

                return await IPersonRepository.CreatePerson(person);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el cliente");
            }
        }
        public async Task<ModelsEF.Cliente> UpdateClientNoUser(ClientEntity clientNewData, ModelsEF.Cliente client)
        {
            try
            {
                client.Referencia = clientNewData.referencia;
                client.IdCiudad = clientNewData.idCiudad;
                client.Telefono1 = clientNewData.telefono1;
                client.Telefono2 = clientNewData.telefono2;
                

                return await IClientRepository.UpdateClient(client);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el cliente");
            }
        }

        public async Task<Persona> UpdatePersonNoUser(ClientEntity clientNewData, Persona person)
        {
            try
            {
                person.Email = clientNewData.email;
                person.Nombre = clientNewData.nombre;
                person.Apellido = clientNewData.apellido;
                person.IdSexo = clientNewData.idSexo;

                return await IPersonRepository.UpdatePerson(person);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el cliente");
            }
        }

        public async Task<Ventum> CreateSale(SaleEntity saleData, int idClient)
        {
            try
            {
                Ventum sale = new();
                sale.IdCliente = idClient;
                sale.Fecha = saleData.fecha;
                sale.IdImpuesto = null;
                sale.Total = saleData.total;
                sale.IdStatus = 1;
                sale.CostosAdicionales = saleData.costosAdicionales;
                sale.MotivoCostosAdicional = saleData.motivoCostosAdicional;
                sale.IdFormaPago = 1;
                sale.Direccion = saleData.direccion;
                sale.Referencia = saleData.referencia;
                sale.IdCiudad = saleData.idCiudad;

                return await ISaleRepository.CreateSale(sale);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al registrar la venta");
            }
        }
        public async Task<DetalleVentum> CreateDetail(SaleDetailEntity detail, int idSale)
        {
            try
            {
                DetalleVentum saleDetail = new();
                saleDetail.IdVenta = idSale;
                saleDetail.Cantidad = detail.cantidad;
                saleDetail.Precio = detail.precio;
                saleDetail.IdPromocion = detail.idPromocion;
                saleDetail.IdProducto = detail.idProducto;
                saleDetail.Descuento = detail.descuentoTotal;

                return await ISaleRepository.CreateDetail(saleDetail);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al registrar el detalle de venta");
            }
        }
        public async Task<string> UpdateStateSale(int IdStatus, int idSale)
        {
            try
            {
                SaleEntity sale = IMapper.Map<SaleEntity>(
                    await ISaleRepository.FindSaleById(idSale)
                );
                
                sale.status = IdStatus;
                return await ISaleRepository.UpdateSale(
                    IMapper.Map<Ventum>(sale)
                );
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el estado de la venta.");
            }
        }
        public async Task<List<SalesStatus>> GetAllSalesStatus()
        {
            try
            {
                var list = await ISaleRepository.GetAllSalesStatus();
                List<SalesStatus> sale = IMapper.Map<List<SalesStatus>>(
                    list
                );

                return sale;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un al obtener los estados de las ventas.");
            }
        }
        public async Task<List<SalesCompleteEntity>> GetAllSales()
        {
            try
            {
                var sales = await ISaleRepository.GetAllSales();
                List<SalesCompleteEntity> salesCompleteInfo = new();
                foreach (var item in sales)
                {

                    salesCompleteInfo.Add(new SalesCompleteEntity(
                        IMapper.Map<SaleEntity>(item),
                        IMapper.Map<List<SaleDetailEntity>>(item.DetalleVenta)
                        ));
                }
                return salesCompleteInfo;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un al obtener los estados de las ventas.");
            }
        }
        public async Task<List<SalesCompleteEntity>> GetAllSalesByIdClient(int idClient)
        {
            try
            {
                var sales = await ISaleRepository.FindAllSalesByIdClient(idClient);
                List<SalesCompleteEntity> salesCompleteInfo = new();
                foreach (var item in sales)
                {

                    salesCompleteInfo.Add(new SalesCompleteEntity(
                        IMapper.Map<SaleEntity>(item),
                        IMapper.Map<List<SaleDetailEntity>>(item.DetalleVenta)
                        ));
                }
                return salesCompleteInfo;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un al obtener datos de las ventas.");
            }
        }
        public async Task<List<AccountsEntity>> GetAllBanks()
        {
            try
            {
                var bankName = await this.IBankInfoRepository.GetBankInfo();

                return IMapper.Map<List<AccountsEntity>>(bankName);
            }
            catch (Exception err)
            {
                return new();
            }
        }
        private async Task<float> getDiscountTotalByProductAndPromotion(SaleDetailEntity sale)
        {
            try
            {
                var promInPrdct = await this.IProductoRepository
                    .getPromotionByIdAndProduct(sale.idPromocion.Value, sale.idProducto);

                if (promInPrdct == null || promInPrdct.IdProductoNavigation == null
                    || promInPrdct.IdPromocionNavigation == null)
                    return 0;

                var product = promInPrdct.IdProductoNavigation;
                var promotion = promInPrdct.IdPromocionNavigation;

                var discount = promotion.PorcentajePromo != null
                    ? ((promotion.PorcentajePromo / 100) * product.Precio)
                    : promotion.DsctoMonetario != null
                    ? promotion.DsctoMonetario
                    : 0;

                return (float)Math.Round((decimal)(sale.cantidad * discount), 2);

            }
            catch (Exception err)
            {
                return 0;
            }
        }
    }
}

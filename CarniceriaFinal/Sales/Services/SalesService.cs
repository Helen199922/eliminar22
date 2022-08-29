using AutoMapper;
using CarniceriaFinal.Cliente.Models;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Core.Email.DTOs;
using CarniceriaFinal.Core.Email.Services.IServices;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Marketing.Services.IService;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IServicios;
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
        private readonly IMembershipService IMembershipService;
        private readonly IMembershipRepository IMembershipRepository;
        private readonly IMeasureUnitService IMeasureUnitService;
        private readonly IMapper IMapper;
        public SalesService(IUserRepository IUserRepository, IPersonRepository IPersonRepository, 
            IClientRepository IClientRepository, ISaleRepository ISaleRepository, 
            IMapper IMapper, IProductoRepository IProductoRepository,
            ICitiesServices ICitiesServices, IEmailService IEmailService,
            IBankInfoRepository IBankInfoRepository,
            IMembershipService IMembershipService,
            IMembershipRepository IMembershipRepository,
            IMeasureUnitService IMeasureUnitService)
        {
            this.IUserRepository = IUserRepository;
            this.IPersonRepository = IPersonRepository;
            this.IClientRepository = IClientRepository;
            this.ISaleRepository = ISaleRepository;
            this.IProductoRepository = IProductoRepository;
            this.ICitiesServices = ICitiesServices;
            this.IEmailService = IEmailService;
            this.IBankInfoRepository = IBankInfoRepository;
            this.IMembershipService = IMembershipService;
            this.IMembershipRepository = IMembershipRepository;
            this.IMapper = IMapper;
            this.IMeasureUnitService = IMeasureUnitService;
        }
        public async Task<SalesUserInformationResponse> CreateSaleNoUser(SaleNoUserRequestEntity sale)
        {
            try
            {
                sale.isAuthUser = 0;

                if (sale.cliente == null || sale.cliente.cedula == null || sale.cliente.apellido == null
                    || sale.cliente.direccion == null || sale.cliente.email == null)
                    throw RSException.BadRequest("Por favor, vuelva a intentar. Datos incompletos");

                var person = await IPersonRepository.GetPersonByIdentification(sale.cliente.cedula);
                var client = await IClientRepository.GetClientByIdentification(sale.cliente.cedula);
                var user = await IUserRepository.GetUserByIdIndentificationPerson(sale.cliente.cedula);


                float finalAmount = (float)0;
                float subTotal = 0;
                float discount = 0;

                List<DetailProductsEntity> salesDetails = new();
                foreach (var item in sale.detalleVenta)
                {
                    float discountInDetail = 0;
                    var productValue = await this.IProductoRepository.FindProductById(item.idProducto);
                    item.idUnidad = productValue.IdUnidad;
                    if (productValue == null || productValue.Stock == 0 || productValue.Stock < item.cantidad) 
                        throw RSException.BadRequest("El producto no se encontró o no hay stock disponible");

                    
                    if (item.idPromocion != null)
                    {
                        discountInDetail = await getDiscountTotalByProduct(item);
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
                        finalAmount = Math.Round((productValue.Precio.Value * item.cantidad), 2).ToString(),
                        discount = discountInDetail > 0 ? Math.Round((discountInDetail), 2).ToString() : null,
                        typeDiscount = item.idPromocion != null ? "Promoción vigente" : null
                    });
                }

                //Verificar si el cliente no está registrado como un usuario con clave y contraseña
                //var user = await IUserRepository.GetUserByIdIndentificationPerson(sale.cedula);
                //if (user != null)
                //{
                //    throw RSException.Unauthorized("Ya existe un usuario registrado con estas credenciales. Por favor, inicie sesión.");
                //}
                //var client = await IClientRepository.GetClientByIdentification(sale.cliente.cedula);

                //actualizar persona
                int idClient = 0;

                if(person == null)
                {
                    ClientEntity personToAdd = new()
                    {

                        email = sale.cliente.email,
                        nombre = sale.cliente.nombre,
                        apellido = sale.cliente.apellido,
                        cedula = sale.cliente.cedula,
                    };
                    person = await CreatePerson(personToAdd);
                    client = await CreateClient(person.IdPersona);
                    idClient = client.IdCliente;
                }

                if (client == null)
                {
                    var clientCreated = await CreateClient(user.IdPersona.Value);
                    client = clientCreated;
                    idClient = clientCreated.IdCliente;
                }
                else
                {
                    idClient = client.IdCliente;
                }

                Ventum saleCreated = await CreateSale(idClient, finalAmount, sale);
                
                //Crear datalle
                foreach (var detail in sale.detalleVenta)
                {
                    await CreateDetail(detail, saleCreated.IdVenta);
                    await IProductoRepository.DisminuirStock(detail.idProducto, detail.cantidad);
                }


                //var email = await this.IEmailService.SendEmailToProductRequest(new EmailProductsRequest
                //{
                //    numPedido = saleCreated.IdVenta.ToString(),
                //    amount= finalAmount,
                //    email=sale.cliente.email,
                //    userName=sale.cliente.nombre,
                //    accounts= await GetAllBanks(),
                //    discount = discount.ToString(),
                //    subTotal = Math.Round(subTotal, 2).ToString(),
                //    productDetail = salesDetails,
                //    transporte = sale.costosAdicionales.ToString()
                //});

                return new()
                {
                    numVenta = saleCreated.IdVenta,
                    timeStart = saleCreated.Fecha == null ? DateTime.Now : saleCreated.Fecha.Value,
                    timeEnd = saleCreated.FechaFinal == null ? DateTime.Now : saleCreated.FechaFinal.Value
                };

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
        public async Task<SalesUserInformationResponse> CreateSaleUser(SaleNoUserRequestEntity sale)
        {
            try
            {
                sale.isAuthUser = 1;
                if (sale.cliente.cedula == null)
                    RSException.BadRequest("Información con error. Por favor, vuelva a intentar");

                //Verificar si el cliente está registrado como un usuario con clave y contraseña

                await this.isValidCotizacion(sale);

                var user = await IUserRepository.GetUserByIdIndentificationPerson(sale.cliente.cedula);

                if (user == null)
                    throw RSException.Unauthorized("Ha ocurrido un error con su cuenta de usuario. Por favor, contactarse con el administrador.");
                


                var person = await IPersonRepository.GetPersonByIdentification(sale.cliente.cedula);
                var client = await IClientRepository.GetClientByIdentification(sale.cliente.cedula);

                sale.cliente.email = person.Email;
                sale.cliente.nombre = person.Nombre;
                sale.cliente.apellido = person.Apellido;
                sale.cliente.direccion = person.Direccion1;


                float finalAmount = 0;
                float subTotal = 0;
                float discount = 0;
                List<DetailProductsEntity> salesDetails = new();
                foreach (var item in sale.detalleVenta)
                {
                    float discountInDetail = 0;
                    var productValue = await this.IProductoRepository.FindProductById(item.idProducto);
                    item.idUnidad = productValue.IdUnidad;
                    if (productValue == null || productValue.Stock == 0 || productValue.Stock < item.cantidad)
                        throw RSException.BadRequest("El producto no se encontró");

                    if (item.idPromocion != null || item.idMembresiaInUser != null)
                    {
                        discountInDetail = await getDiscountTotalByProduct(item);
                        if (discountInDetail == 0)
                        {
                            item.idPromocion = null;
                            item.idMembresiaInUser = null;
                        }
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
                        finalAmount = Math.Round((productValue.Precio.Value * item.cantidad), 2).ToString(),
                        discount = discountInDetail > 0 ? Math.Round((discountInDetail), 2).ToString() : null,
                        typeDiscount = item.idPromocion != null ? "Promoción vigente" : "Membresía"
                    });

                }

                
                
                
                
                //actualizar persona
                int idClient = 0;
                if (client == null)
                {
                    var clientCreated = await CreateClient(user.IdPersona.Value);
                    idClient = clientCreated.IdCliente;
                }
                else
                {
                    idClient = client.IdCliente;
                }

                Ventum saleCreated = await CreateSale(idClient, finalAmount, sale);
                DateTime expireTransa = DateTime.Now;
                //Crear datella

                foreach (var detail in sale.detalleVenta)
                {
                    await CreateDetail(detail, saleCreated.IdVenta);
                    await IProductoRepository.DisminuirStock(detail.idProducto, detail.cantidad);
                }

                //var email = await this.IEmailService.SendEmailToProductRequest(new EmailProductsRequest
                //{
                //    numPedido = saleCreated.IdVenta.ToString(),
                //    amount = finalAmount,
                //    email = sale.cliente.email,
                //    userName = sale.cliente.nombre,
                //    accounts = await GetAllBanks(),
                //    discount = Math.Round(discount, 2).ToString(),
                //    subTotal = Math.Round(subTotal, 2).ToString(),
                //    productDetail = salesDetails,
                //    transporte = sale.costosAdicionales.ToString()
                //});

                //return email;
                return new()
                {
                    numVenta = saleCreated.IdVenta,
                    timeStart = saleCreated.Fecha == null ? DateTime.Now : saleCreated.Fecha.Value,
                    timeEnd = saleCreated.FechaFinal == null ? DateTime.Now : saleCreated.FechaFinal.Value
                };


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

        public async Task<ModelsEF.Cliente> CreateClient(int idPerson)
        {
            try
            {
                ModelsEF.Cliente client = new();
                client.Referencia = "";
                client.IdCiudad = null;
                client.Telefono1 = "";
                client.Telefono2 = "";
                client.Direccion = "";
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
                person.IdSexo = 1;
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

        public async Task<Ventum> CreateSale(int? idClient, float finalAmount, SaleNoUserRequestEntity saleInfo)
        {
            try
            {
                Ventum sale = new();
                sale.IdCliente = idClient;
                sale.Fecha = DateTime.Now;
                sale.IdImpuesto = null;
                sale.Total = finalAmount;
                sale.IdStatus = 1;
                sale.CostosAdicionales = 0;
                sale.MotivoCostosAdicional = "";
                sale.IdFormaPago = 1;
                sale.Direccion = "";
                sale.Referencia = "";
                sale.IdCiudad = 1;
                sale.IsAuthUser = saleInfo.isAuthUser;
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
                saleDetail.IdMembresiaInUsuario = detail.idMembresiaInUser;
                saleDetail.IdProducto = detail.idProducto;
                saleDetail.Descuento = detail.descuentoTotal;
                saleDetail.IdUnidad = detail.idUnidad;

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
        public async Task<SalesUserInformationResponse> GetStatusByIdSale(int idSale)
        {
            try
            {
                var sale = await ISaleRepository.GetStatusByIdSale(idSale);

                if (sale == null)
                    throw RSException.NoData("No se encontró información");

                return new()
                {
                    numVenta = sale.IdVenta,
                    timeStart = sale.Fecha.Value,
                    timeEnd = sale.FechaFinal.Value
                };

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un al obtener datos de la venta.");
            }
        }
        private async Task<float> getDiscountTotalByProduct(SaleDetailEntity sale)
        {
            try
            {
                float? discount = 0;

                if (sale.idPromocion != null)
                {
                    var promInPrdct = await this.IProductoRepository
                        .getPromotionByIdAndProduct(sale.idPromocion.Value, sale.idProducto);

                    if (promInPrdct == null || promInPrdct.IdProductoNavigation == null
                        || promInPrdct.IdPromocionNavigation == null)
                        return 0;

                    var product = promInPrdct.IdProductoNavigation;
                    var promotion = promInPrdct.IdPromocionNavigation;

                        discount = promotion.PorcentajePromo != null
                        ? ((promotion.PorcentajePromo / 100) * product.Precio)
                        : promotion.DsctoMonetario != null
                        ? promotion.DsctoMonetario
                        : 0;
                }

                if (sale.idMembresiaInUser != null)
                {
                    var membershipDetail = await this.IMembershipRepository
                        .GetMembershipDetail(sale.idMembresiaInUser.Value);
                    var promInPrdct = await this.IProductoRepository
                        .ProductById(sale.idProducto);

                    var discountPercent = membershipDetail.PorcentajeDescuento;

                    discount = discountPercent != null
                    ? ((discountPercent / 100) * promInPrdct.Precio)
                    : 0;

                    if(discount > membershipDetail.MontoMaxDescPorProducto)
                    {
                        discount = membershipDetail.MontoMaxDescPorProducto;
                    }
                }


                return (float)Math.Round(discount.Value, 2)*(sale.cantidad);

            }
            catch (Exception err)
            {
                return 0;
            }
        }

        private async Task<Boolean> isValidCotizacion(SaleNoUserRequestEntity sale)
        {
            try
            {
                var members = sale.detalleVenta.Where(x => x.idMembresiaInUser != null).ToList();
                var details = sale.detalleVenta.Where(x => x.idPromocion != null && x.idMembresiaInUser != null).ToList();

                if(details.Count > 0) 
                   throw RSException.BadRequest("Existe una promocion y membresia registrada en un mismo producto.");

                if (members.Count == 0) 
                    return true;

                var cedula = sale.cliente.cedula;

                if (cedula == null) 
                    RSException.BadRequest("Tuvimos un error para validar su membresia.");

                var userInformation = await this.IUserRepository.GetUserByIdIndentificationPerson(cedula);

                return await IMembershipService.isValidMembership(members, userInformation.IdUsuario);


            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al validar la cotización");
            }
        }
        public async Task<SaleDetailCotizacionEntity> FindCompleteSaleByIdSale(int idSale)
        {
            try
            {
                var saleDetail = await this.ISaleRepository.FindCompleteSaleByIdSale(idSale);
                if (saleDetail == null) throw RSException.NoData("No hemos encontrado información sobre la venta");
                var productUnidad = await this.IMeasureUnitService.GetAllMeasureUnit();


                SaleDetailCotizacionEntity sale = new();
                sale.idVenta = saleDetail.IdVenta;
                sale.total = saleDetail.Total.Value;
                sale.dateCreated = saleDetail.Fecha;
                sale.products = new();

                foreach (var product in saleDetail.DetalleVenta)
                {
                    sale.descuentoTotalVenta += (product.Descuento == null ? 0 : product.Descuento.Value);
                    sale.subtotalVenta += (product.Precio.Value * product.Cantidad.Value);
                    
                    var productUnit = productUnidad.Where(x => x.idUnidad == product.IdUnidad).Select(x => x.unidad).FirstOrDefault();

                    var discount = (product.Descuento == null || product.Descuento == 0 ? 0 : (float)Math.Round(product.Descuento.Value / product.Cantidad.Value, 2));
                    sale.products.Add(new ProductsDetailCotizacionEntity()
                    {
                        cantidad = product.Cantidad.Value,
                        titulo = product.IdProductoNavigation.Titulo,
                        unidad = productUnit == null ? "lb" : productUnit,
                        descuentoTotalProducto = discount,
                        motivoDesc = product.IdPromocion != null ? "Promocion" : "Membresía de Usuario",
                        precioFinalProducto = (float)Math.Round(((product.Precio.Value * product.Cantidad.Value) - (product.Descuento == null ? 0 : product.Descuento.Value)) / product.Cantidad.Value, 2)
                    });
                }
                sale.subtotalVenta = (float)Math.Round(sale.subtotalVenta, 2);
                sale.descuentoTotalVenta = (float)Math.Round(sale.descuentoTotalVenta, 2);
                return sale;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener los detalles de la venta");
            }
        }
        public async Task<List<SaleDetailCotizacionEntity>> FindAllCompleteSaleByIdClient(int idUser)
        {
            try
            {

                var client = await IClientRepository.GetClientByIdUser(idUser);

                if (client == null) return new List<SaleDetailCotizacionEntity>();


                var saleDetail = await this.ISaleRepository.FindAllCompleteSaleByIdClient(client.IdCliente);

                List<SaleDetailCotizacionEntity> sales = new();
                var productUnidad = await this.IMeasureUnitService.GetAllMeasureUnit();


                foreach (var saleVal in saleDetail)
                {
                    SaleDetailCotizacionEntity sale = new();
                    sale.idVenta = saleVal.IdVenta;
                    sale.total = saleVal.Total.Value;
                    sale.dateCreated = saleVal.Fecha;
                    sale.products = new();

                    foreach (var product in saleVal.DetalleVenta)
                    {
                        var discount = (product.Descuento == null || product.Descuento == 0 ? 0 : (float)Math.Round(product.Descuento.Value / product.Cantidad.Value, 2));

                        var productUnit = productUnidad.Where(x => x.idUnidad == product.IdUnidad).Select(x => x.unidad).FirstOrDefault();

                        sale.descuentoTotalVenta += (product.Descuento == null ? 0 : product.Descuento.Value);
                        sale.subtotalVenta += (product.Precio.Value * product.Cantidad.Value);
                        sale.products.Add(new ProductsDetailCotizacionEntity()
                        {
                            cantidad = product.Cantidad.Value,
                            titulo = product.IdProductoNavigation.Titulo,
                            descuentoTotalProducto = discount,
                            unidad = productUnit == null ? "lb" : productUnit,
                            motivoDesc = product.IdPromocion != null ? "Promocion" : "Membresía de Usuario",
                            precioFinalProducto = (float)Math.Round(((product.Precio.Value * product.Cantidad.Value) - (product.Descuento == null ? 0 : product.Descuento.Value)) / product.Cantidad.Value, 2)
                        });
                    }
                    sale.descuentoTotalVenta = (float)Math.Round(sale.descuentoTotalVenta, 2);
                    sale.subtotalVenta = (float)Math.Round(sale.subtotalVenta, 2);
                    sales.Add(sale);

                }

                return sales;

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Obtener lista detalle de las ventas del cliente");
            }
        }

    }
}

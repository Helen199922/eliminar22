using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.Marketing.Repository.IRepository;
using CarniceriaFinal.Productos.Repository;
using CarniceriaFinal.Sales.DTOs;
using CarniceriaFinal.Sales.Repository.IRepository;
using CarniceriaFinal.Sales.Services.IServices;

namespace CarniceriaFinal.Sales.Services
{
    public class SalesAdmServices: ISalesAdmServices
    {
        private readonly ISalesAdmRepository ISalesAdmRepository;
        private readonly IMembershipRepository IMembershipRepository;
        private readonly IProductoRepository IProductoRepository;
        private readonly IMapper IMapper;
        public SalesAdmServices(
            IMapper IMapper,
            ISalesAdmRepository ISalesAdmRepository,
            IMembershipRepository IMembershipRepository,
            IProductoRepository IProductoRepository
        )
        {
            this.IMapper = IMapper;
            this.ISalesAdmRepository = ISalesAdmRepository;
            this.IMembershipRepository = IMembershipRepository;
            this.IProductoRepository = IProductoRepository;
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
        public async Task<ResumeSaleDetail> GetDetailByIdSale(int idSale)
        {
            try
            {
                Boolean WARN_STOCK = false;
                Boolean WARN_PROMO = false;
                Boolean WARN_MEMBER_EXPIRE = false;
                Boolean WARN_MEMBER_PRODUCTS_COMPLETE = false;
                //Tipo de incosistencias: 
                // Stock insuficiente
                // El producto se deshabilitó
                // Caducó la promoción
                // Caducó la membresía
                // Completó el total planificado una membresía

                var blockNextStep = "";
                ResumeSaleDetail resumen = new ResumeSaleDetail();
                List<Inconsistency> inconsistencies = new();

                var details = await ISalesAdmRepository.GetDetailByIdSale(idSale);
                resumen.details = details;

                foreach (var detail in details)
                {
                    if (detail.idMembresiaInUser != null)
                        await this.IMembershipRepository.AdmMemberTimesByIdUserInMembership(detail.idMembresiaInUser.Value);

                    //Consultar stock - inconsistencia en Stock
                    var productInfo = await ISalesAdmRepository.GetInfoProductByIdProduct(detail.idProduct);
                    if(detail.cantidad > productInfo.Stock && !WARN_STOCK)
                    {
                        WARN_STOCK = true;
                        inconsistencies.Add(new()
                        {
                            descripcion = "Stock Insuficiente",
                            tipoInconsistencia = "Stock Insuficiente"
                        });
                    }


                    if (detail.detailDiscount != null && DateTime.Compare(detail.detailDiscount.fechaFin, DateTime.Now) < 0)
                    {
                        //Analizar Promoción - Expirada
                        if (detail.idPromocion != null && !WARN_PROMO)
                        {
                            WARN_PROMO = true;
                            inconsistencies.Add(new()
                            {
                                descripcion = "Promoción Caducada",
                                tipoInconsistencia = "Promoción Caducada"
                            });
                        }

                        //Analizar Membresía - Expirada
                        if (detail.idMembresiaInUser != null && !WARN_MEMBER_EXPIRE)
                        {
                            WARN_MEMBER_EXPIRE = true;
                            inconsistencies.Add(new()
                            {
                                descripcion = "Membresía Caducada",
                                tipoInconsistencia = "Membresía Caducada"
                            });
                        }
                    }

                    //Analizar Promoción - Caducada
                    if (detail.detailDiscount != null && detail.idPromocion != null && !WARN_PROMO && detail.detailDiscount.status == 0)
                    {
                        WARN_PROMO = true;
                        inconsistencies.Add(new()
                        {
                            descripcion = "Promoción Caducada",
                            tipoInconsistencia = "Promoción Caducada"
                        });
                    }
                    //Analizar Membresía - Más productos en membresía
                    if (detail.idMembresiaInUser != null && !WARN_MEMBER_PRODUCTS_COMPLETE)
                    {
                        WARN_MEMBER_PRODUCTS_COMPLETE = true;
                        var membershipDetail = await this.IMembershipRepository
                            .GetMembershipDetailToAdmSales(detail.idMembresiaInUser.Value);

                        if(membershipDetail == null && membershipDetail.Status == 3)
                        {
                            blockNextStep = "Detectamos una inconsistencia. Tal vez, se rechazó la venta que permitía ofrecerle este beneficio. Por lo cual dicha venta no se puede realizar.";
                            inconsistencies.Add(new()
                            {
                                descripcion = "Membresía de Usuario no Existe. Tal vez, se rechazó la venta que permitía ofrecerle este beneficio.",
                                tipoInconsistencia = "Membresía de Usuario no Existe. Tal vez, se rechazó la venta que permitía ofrecerle este beneficio."
                            });
                        }

                        if (
                                (membershipDetail.IdMembresiaNavigation.CantProductosMembresia - membershipDetail.CantProductosComprados)
                                < detail.cantidad
                           )
                        {
                            var disponibilidiad = (membershipDetail.IdMembresiaNavigation.CantProductosMembresia) - (membershipDetail.CantProductosComprados);
                            blockNextStep = String
                                .Format("Hay más productos de los que soporta esta membresía: registramos {0} productos dentro de la membresía del cliente y al agregar {1} superamos la cant. máxima de: {2}",
                                membershipDetail.CantProductosComprados, detail.cantidad, membershipDetail.CantProductosComprados, membershipDetail.IdMembresiaNavigation.CantProductosMembresia);

                            inconsistencies.Add(new()
                            {
                                descripcion = "Capacidad Membresía Superada",
                                tipoInconsistencia = String.Format("Capacidad Membresía Superada - Tiene ({0}) productos disp. de ({1}) que se requieren para completar la compra.", 
                                                        disponibilidiad, detail.cantidad
                                                    )
                            });

                        }
                    }

                }
                var status = await ISalesAdmRepository.GetSalesStatusAdm();
                var sale = await ISalesAdmRepository.GetSalesById(idSale);

                resumen.razonBloqueo = blockNextStep;
                resumen.inconsistencias = inconsistencies;
                resumen.fechaActual = DateTime.Now;
                resumen.fechaRealizacion = sale.Fecha.Value;
                resumen.actuallyStatus = new()
                {
                    id = sale.IdStatus.Value,
                    titulo = sale.IdStatusNavigation.Status
                };
                var listSTatus =  status.Select(x => new StatusSale()
                {
                    id = x.IdVentaStatus,
                    titulo = x.Status
                }).ToList();

                resumen.listStatus = listSTatus;
                return resumen;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener el detalle de la venta");
            }
        }
        public async Task<string> attendSale(int idSale)
        {
            try
            {
                var details = await ISalesAdmRepository.GetDetailByIdSale(idSale);

                var valuesWithMembership = details.Where(x => x.idMembresiaInUser != null).ToList();
                if(valuesWithMembership.Count > 0)
                {
                    var membershipUser = await IMembershipRepository
                        .GetMembershipDetailToAdmSales(valuesWithMembership[0].idMembresiaInUser.Value);

                    foreach (var value in valuesWithMembership)
                    {
                        if(value.cantidad > 
                                (membershipUser.IdMembresiaNavigation.CantProductosMembresia - membershipUser.CantProductosComprados)
                         )
                        {
                            throw RSException.BadRequest("lamentablemebte, hay una inconsistencia en cuanto a la membresía. Revise los detalles de la compra");
                        }
                    }
                }

                var isAttend = await ISalesAdmRepository.attendSaleByIdSale(idSale);

                if (isAttend != true) throw RSException.NoData("Tuvimos un error al registrar la venta.");

                foreach (var detail in details)
                {
                    if (detail.idMembresiaInUser != null)
                    {
                       await this.IMembershipRepository.AddProductsToMembershipByIdUser(detail.idMembresiaInUser.Value, detail.cantidad);
                    }

                    //Reducir Stock
                    await this.IProductoRepository.DisminuirStock(detail.idProduct, detail.cantidad);
                }

                //Revisar si el cliente aplica para la membresía o se le acabó
                return await this.AdmNewMemberWhenAttendSaleByIdSale(idSale);
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
        public async Task<string> declineSale(int idSale)
        {
            try
            {
                var details = await ISalesAdmRepository.GetDetailByIdSale(idSale);

                var isdeclined = await ISalesAdmRepository.declineSaleByIdSale(idSale);

                if (isdeclined != true) throw RSException.NoData("Tuvimos un error al registrar la venta.");

                foreach (var detail in details)
                {
                    if (detail.idMembresiaInUser != null)
                    {
                        await this.IMembershipRepository.RemoveProductsToMembershipByIdUser(detail.idMembresiaInUser.Value, detail.cantidad);
                    }

                    //Aumentar Stock
                    await this.IProductoRepository.AumentarStock(detail.idProduct, detail.cantidad);
                }

                //Revisar si el cliente pierde membresía con esta reducción
                return await this.AdmActuallyMemberWhenDeclineSaleByIdSale(idSale);
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
        private async Task<string> AdmNewMemberWhenAttendSaleByIdSale(int idSale)
        {
            try
            {
                //encontrar el idClient
                var sale = await ISalesAdmRepository.GetSalesById(idSale);
                var userId = await ISalesAdmRepository.GetUserIdByIdSale(idSale);

                var amount = await IMembershipRepository.AmountToMembershipInUser(userId, sale.IdCliente.Value);
                var membreship = (await IMembershipRepository.GetAllMembershipDetail()).OrderByDescending(x => x.MontoMinAcceso).ToList();
                foreach (var item in membreship)
                {
                    if(item.MontoMinAcceso <= amount)
                    {
                        await IMembershipRepository.RegisterMembershipToUser(userId, item);
                        return "Se ha agregado una nueva membresía para el usuario: " + item.Titulo;
                    }
                }

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al ver beneficios del usuario");
            }
        }
        private async Task<string> AdmActuallyMemberWhenDeclineSaleByIdSale(int idSale)
        {
            try
            {
                //encontrar el idClient
                var sale = await ISalesAdmRepository.GetSalesById(idSale);
                var userId = await ISalesAdmRepository.GetUserIdByIdSale(idSale);

                var existLastMembership = await IMembershipRepository.GetLastMembershipByIdUser(userId);
                if (existLastMembership == null) return "";

                var amount = await IMembershipRepository.AmountToMemberUserDeclineSale(userId, sale.IdCliente.Value);


                var membreship = (await IMembershipRepository.GetAllMembershipDetail())
                    .Where(x => x.IdMembresia == existLastMembership.IdMembresia)
                    .FirstOrDefault();


                if(membreship.MontoMinAcceso > amount)
                {
                    await IMembershipRepository.BlockMembershipToUser(existLastMembership.IdMembresiaInUsuario);
                    return "Se ha bloqueado la membresía vigente en esta compra del usuario porque no cumple con le monto minimo para formar parte de ella. Esto se debe al rechazar la compra actual: " + membreship.Titulo;
                }

                return "";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al ver beneficios del usuario");
            }
        }
    }
}

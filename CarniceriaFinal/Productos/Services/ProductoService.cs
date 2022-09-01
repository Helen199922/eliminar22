using AutoMapper;
using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.IRepository;
using CarniceriaFinal.Productos.IServicios;
using CarniceriaFinal.Productos.Models;
using CarniceriaFinal.Productos.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Servicios
{
    public class ProductoService: IProductoService
    {
        public readonly IProductoRepository IProductoRepo;
        public readonly ICategoriaRepository ICategoriaRepo;
        public readonly ICategoriaService ICategoriaService;
        public readonly IPromocionRepository IPromocionRepository;
        public readonly IUnidadMedidaRepository IUnidadMedidaRepository;
        private readonly IMapper IMapper;

        public ProductoService(IProductoRepository ProductoRepo, ICategoriaRepository ICategoriaRepo, 
            IPromocionRepository IPromocionRepository, IUnidadMedidaRepository IUnidadMedidaRepository,
            ICategoriaService ICategoriaService, IMapper IMapper)
        {
            this.IProductoRepo = ProductoRepo;
            this.ICategoriaRepo = ICategoriaRepo;
            this.IPromocionRepository = IPromocionRepository;
            this.IUnidadMedidaRepository = IUnidadMedidaRepository;
            this.ICategoriaService = ICategoriaService;
            this.IMapper = IMapper;
        }

        public async Task<List<ProductTableAdminEntity>> GetSimpleProductos()
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetSimpleProducts();
                List<ProductTableAdminEntity> products = new();
                foreach (var item in productsRepo)
                {
                    var product = IMapper.Map<ProductTableAdminEntity>(item);
                    products.Add(product);
                    var lastProduct = products.Last();
                    lastProduct.NamePromotion = "";
                    
                    lastProduct.NameCategories = new();

                }


                return products;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage(err.Message);
            }

        }
        public async Task<ProductAdminCompleteEntity> GetDetailAdminProductoById(int idProduct)
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetDetailAdminProductoById(idProduct);

                if(productsRepo == null)
                    throw new RSException("No content", 404, "No hemos encontrado el producto solicitado");

                ProductAdminCompleteEntity productComplete = new();
                var product = IMapper.Map<ProductEntity>(productsRepo);
                List<ProductEntity> products = new();
                products.Add(product); ;
                var responsePromotion = await promotionConvert(products);

                productComplete.product = responsePromotion[0];
                productComplete.unidadMedida = IMapper.Map<MeasureUnitEntity>(productsRepo.IdUnidadNavigation);
                productComplete.detail = IMapper.Map<List<ProductDetailEntity>>(productsRepo.DetalleProductos);
                var categoriesResponse = await this.ICategoriaService.GetAllCategoriesByProductId(idProduct);
                productComplete.categories = IMapper.Map<List<CategoriaProductoEntity>>(categoriesResponse);

                return productComplete;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage(err.Message);
            }
        }
        public async Task<List<ProductEntity>> GetProductos()
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetProductos();
                List<ProductEntity> products = IMapper.Map<List<ProductEntity>>(productsRepo);
                
                return await promotionConvert(products);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage(err.Message);
            }
        }
        public async Task<List<ProductSimpleIdsEntity>> GetSimpleProductsIds()
        {
            try
            {
                return await this.IProductoRepo.GetSimpleProductsIds();
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener datos de los productos");
            }
        }
        public async Task<List<ProductEntity>> GetProductosInCar(List<int> idProducts)
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetProductosInCar(idProducts);
                List<ProductEntity> products = IMapper.Map<List<ProductEntity>>(productsRepo);

                return await promotionConvert(products);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage(err.Message);
            }
        }

        public async Task<string> SaveProduct(ProductSaveRequestEntity product)
        {
            try
            {
                product.Status = 1;
                Boolean isNullPromotion = false;

                if (IUnidadMedidaRepository.GetUnidadMedidaById(product.IdUnidad).Result == null)
                    throw RSException.NoData("No hemos encontrado la unidad de medida solicitada");
                
                var productRepo = IMapper.Map<Producto>(product);


                var productResponse = await this.IProductoRepo.SaveProduct(productRepo);

                foreach (var detail in product.detail)
                {
                    detail.IdProducto = productResponse.IdProducto;
                }
                if(product.detail != null && product.detail.Count > 0)
                    await this.SaveDetails(product.detail);

                return "Producto guardado correctamente";
            }
            catch(RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception err)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }

        public async Task<string> SaveDetails(List<ProductDetailEntity> detailProduct)
        {
            try
            {
                var productRepo = IMapper.Map<List<DetalleProducto>>(detailProduct);

                if (IProductoRepo.ProductById(detailProduct[0].IdProducto).Result == null)
                    throw RSException.NoData("No hemos encontrado el producto para guardar el detalle");


                return await this.IProductoRepo.SaveDetalles(productRepo);

            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el producto");
            }
        }
        public async Task<ProductDetailEntity> SaveOneDetail(ProductDetailEntity productDetail)
        {
            try
            {
                var productRepo = IMapper.Map<DetalleProducto>(productDetail);
                var detail = await IProductoRepo.SaveOneProductDetail(productRepo);

                return IMapper.Map<ProductDetailEntity>(detail);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al guardar el detalle de 1 producto");
            }
        }

        public async Task<List<ProductEntity>> promotionConvert(List<ProductEntity> products)
        {
            try
            {

                foreach (var product in products)
                {
                    try
                    {
                        var promotion = await IProductoRepo.promotionConvert(product.IdProducto ?? 0);
                        if (promotion == null)
                            continue;

                        product.ProductPromotionEntity = new();
                        product.ProductPromotionEntity.idPromotion = promotion.IdPromocion;
                        product.ProductPromotionEntity.oldValue = product.Precio;
                        var promotionValue = promotion.PorcentajePromo != null 
                            ? "%" + promotion.PorcentajePromo 
                            : promotion.DsctoMonetario != null
                            ? "$" + promotion.DsctoMonetario  
                            : "";
                        product.ProductPromotionEntity.promotionValue = promotionValue;
                        var newValue = promotion.PorcentajePromo != null
                            ? product.Precio - ((promotion.PorcentajePromo / 100) * product.Precio)
                            : promotion.DsctoMonetario != null
                            ? (product.Precio - promotion.DsctoMonetario)
                            : 0;

                        product.ProductPromotionEntity.newValue = (float?)Math.Round((decimal)newValue.Value, 2);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception err)
            {
            }
            return products;
        }

        public async Task<List<ProductEntity>> FindProductsByCategoryId(int idCategory)
        {
            try
            {
                var products = IMapper.Map<List<ProductEntity>>(
                    await IProductoRepo.FindProductByCategoryId(idCategory)
                );
                return await promotionConvert(products);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al buscar el producto");
            }
        }

        public async Task<string> UpdateProduct(ProductSaveRequestEntity product)
        {
            try
            {
                var products = await IProductoRepo.FindDetailsByIdProduct(product.IdProducto.Value);

                var details = IMapper.Map<List<ProductDetailEntity>>(products);

                List<int> productDetailsIds = new();
                foreach (var item in product.detail)
                {
                    if (item.IdProducto == 0 || item.IdProducto == null) throw RSException.BadRequest("Ingrese correctamente la información");
                    if (item.IdDetalleProducto == null) continue;
                        productDetailsIds.Add(item.IdDetalleProducto.Value);
                }

                foreach (var item in details)
                {
                    if (item.IdDetalleProducto == null) continue;
                    if (!productDetailsIds.Contains(item.IdDetalleProducto.Value))
                    {
                        await IProductoRepo.DeleteProductDetail(item.IdDetalleProducto.Value);
                    }
                }

                foreach (var item in product.detail)
                {
                    await this.UpdateProductDetail(item);
                }

                await IProductoRepo.UpdateProduct(IMapper.Map<Producto>(product));

                return "Producto actualizado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error actualizar el producto");
            }
        }

        public void DeleteProduct(int idProduct)
        {
            try
            {
                IProductoRepo.DeleteProduct(idProduct);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al eliminar el producto");
            }
        }
        public async Task<ProductDetailEntity> UpdateProductDetail(ProductDetailEntity detail)
        {
            try
            {
                if(detail.IdDetalleProducto == null)
                {
                    var product = await IProductoRepo.SaveOneProductDetail(IMapper.Map<DetalleProducto>(detail));
                    return IMapper.Map<ProductDetailEntity>(product);
                } else {
                    var productExist = await IProductoRepo.FindProductDetailById(detail.IdDetalleProducto.Value);
                    if (productExist == null)
                        throw RSException.NoData("No hemos encontrado el detalle " + detail.TituloDetalle + " del producto " + detail.IdProducto);

                    DetalleProducto newProduct = await IProductoRepo.UpdateProductDetail(IMapper.Map<DetalleProducto>(detail));

                    return IMapper.Map<ProductDetailEntity>(newProduct);
                }
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar el detalle del producto");
            }
        }
        public void DeleteProductDetail(int idProductDetail)
        {
            try
            {
                IProductoRepo.DeleteProductDetail(idProductDetail);
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al eliminar el detalle del producto");
            }
        }
        public async Task<string> EnableProduct(int idProduct)
        {
            try
            {
                await IProductoRepo.EnableProduct(idProduct);
                return "Producto activado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al habilitar el producto");
            }
        }
        public async Task<string> UpdateStock(int idProduct, int stock)
        {
            try
            {
                await IProductoRepo.UpdateStock(idProduct, stock);
                return "Stock de producto actualizado correctamente";
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al actualizar stock del producto");
            }
        }
        public async Task<PromotionsInProduct> getAllProductsPromotions()
        {
            try
            {
                PromotionsInProduct productsWithPromo = new();
                productsWithPromo.products = new();

                var promotion = await IProductoRepo.getPromotionActivate();

                if(promotion == null)
                {
                    productsWithPromo.hasPromotion = false;
                    productsWithPromo.finish = DateTime.Now;
                    return productsWithPromo;
                }

                productsWithPromo.hasPromotion = true;
                productsWithPromo.title = promotion.Titulo;
                productsWithPromo.finish = promotion.FechaFin;

                var products = IMapper.Map<List<ProductEntity>>(
                    await IProductoRepo.getAllProductsPromotions()
                );

                var productsDetail = await promotionConvert(products.DistinctBy(x => x.IdProducto).ToList());

                productsWithPromo.products = productsDetail;
                
                return productsWithPromo;
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al obtener productos con promocion");
            }
        }

    }
}

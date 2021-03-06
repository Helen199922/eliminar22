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
                    if (item.IdPromocionNavigation != null)
                        lastProduct.NamePromotion = item.IdPromocionNavigation.TipoPromo;
                    else lastProduct.NamePromotion = "";
                    
                    lastProduct.NameCategories = new();
                    if (item.SubInCategoria == null) continue;

                    foreach (var category in item.SubInCategoria.Distinct())
                    {

                        if (lastProduct.NameCategories.Contains(category.IdCategoriaNavigation.Titulo))
                            continue;
                        lastProduct.NameCategories.Add(category.IdCategoriaNavigation.Titulo);
                    }
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
                productComplete.product = IMapper.Map<ProductEntity>(productsRepo);
                productComplete.promotion = IMapper.Map<PromotionEntity>(productsRepo.IdPromocionNavigation);
                productComplete.unidadMedida = IMapper.Map<MeasureUnitEntity>(productsRepo.IdUnidadNavigation);
                productComplete.detail = IMapper.Map<List<ProductDetailEntity>>(productsRepo.DetalleProductos);
                productComplete.categories = await this.ICategoriaService.GetAllCategoriesAndSubCategoriesByProductId(idProduct);

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
        public async Task<List<ProductCompleteEntity>> GetProductos()
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetProductos();
                List<ProductCompleteEntity> products = new();
                foreach (var item in productsRepo)
                {
                    products.Add(new()
                    {
                        product = IMapper.Map<ProductEntity>(item),
                        promotion = IMapper.Map<PromotionEntity>(item.IdPromocionNavigation),
                        unidadMedida = IMapper.Map<MeasureUnitEntity>(item.IdUnidadNavigation),
                        detail = IMapper.Map<List<ProductDetailEntity>>(item.DetalleProductos)
                    });
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
        public async Task<List<SimpleProductInSubCategory>> GetSimpleProductsByIdSubCategories(int? idSubCategory)
        {
            try
            {
                var productsRepo = await this.IProductoRepo.GetAllProductsSubCategory();
                List<SimpleProductInSubCategory> products = new();

                foreach (var item in productsRepo)
                {
                    if (products.Any(x => x.idProducto == item.IdProducto))
                        continue;

                    var existSubCategoryInProduct = (idSubCategory == null)
                        ? false 
                        : item.SubInCategoria.Where(x => x.IdSubCategoria == idSubCategory)
                            .FirstOrDefault() != null ? true : false;

                    products.Add(new SimpleProductInSubCategory()
                    {


                        idProducto = item.IdProducto,
                        titulo = item.Titulo,
                        isActivated = existSubCategoryInProduct
                    });

                    var valores = item.SubInCategoria;
                    
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

        public async Task<string> SaveProduct(ProductSaveRequestEntity product)
        {
            try
            {
                product.Status = 1;
                Boolean isNullPromotion = false;
                if(product.IdPromocion == null)
                {
                    product.IdPromocion = 1;
                    isNullPromotion = true;
                }
                if (IPromocionRepository.GetPromocionById(product.IdPromocion.Value).Result == null)
                    throw RSException.NoData("No hemos encontrado la promoción solicitada");

                if (IUnidadMedidaRepository.GetUnidadMedidaById(product.IdUnidad).Result == null)
                    throw RSException.NoData("No hemos encontrado la unidad de medida solicitada");
                
                var productRepo = IMapper.Map<Producto>(product);

                if (isNullPromotion)
                {
                    productRepo.IdPromocion = null;
                }
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
        public async Task<List<ProductEntity>> FindProductsBySubCategory(int idCategory, int idSubCategory)
        {
            try
            {
                return IMapper.Map<List<ProductEntity>>(
                    await IProductoRepo.FindProductsBySubCategory(idCategory, idSubCategory)
                );
            }
            catch (RSException err)
            {
                throw new RSException(err.TypeError, err.Code, err.MessagesError);
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al consultar el producto");
            }
        }

        public async Task<List<ProductEntity>> FindProductsByCategoryId(int idCategory)
        {
            try
            {
                return IMapper.Map<List<ProductEntity>>(
                    await IProductoRepo.FindProductByCategoryId(idCategory)
                );
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
    }
}

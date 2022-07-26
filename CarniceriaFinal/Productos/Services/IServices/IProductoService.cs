using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Servicios
{
    public interface IProductoService
    {
        Task<List<ProductCompleteEntity>> GetProductos();
        Task<List<SimpleProductInSubCategory>> GetSimpleProductsByIdSubCategories(int? idSubCategory);
        Task<string> SaveProduct(ProductSaveRequestEntity product);
        Task<string> SaveDetails(List<ProductDetailEntity> detalleProducto);
        Task<List<ProductEntity>> FindProductsBySubCategory(int idCategory, int idSubCategory);
        Task<List<ProductEntity>> FindProductsByCategoryId(int idCategory);
        Task<List<ProductTableAdminEntity>> GetSimpleProductos();
        Task<ProductAdminCompleteEntity> GetDetailAdminProductoById(int idProduct);
        Task<string> UpdateProduct(ProductSaveRequestEntity product);
        void DeleteProduct(int idProduct);
        void DeleteProductDetail(int idProductDetail);
        Task<string> EnableProduct(int idProduct);
        Task<string> UpdateStock(int idProduct, int stock);
        Task<PromotionsInProduct> getAllProductsPromotions();
    }
}

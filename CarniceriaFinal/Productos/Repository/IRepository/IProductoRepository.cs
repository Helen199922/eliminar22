using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetProductos();
        Task<Producto> GetDetailAdminProductoById(int idProduct);
        Task<List<Producto>> GetSimpleProducts();
        Task<Producto> ProductById(int id);
        Task<string> SaveDetalles(List<DetalleProducto> detalleProducto);
        Task<List<Producto>> FindProductsBySubCategory(int idCategory, int idSubCategory);
        Task<Producto> SaveProduct(Producto producto);
        Task<List<Producto>> FindProductByCategoryId(int idCategory);
        Task<Producto> UpdateProduct(Producto product);
        Producto DeleteProduct(int idProduct);
        Task<DetalleProducto> UpdateProductDetail(DetalleProducto detail);
        Task<string> DeleteProductDetail(int idProductDetail);
        Task<DetalleProducto> FindProductDetailById(int idProductDetail);
        Task<DetalleProducto> SaveOneProductDetail(DetalleProducto detalleProducto);
        Task<List<DetalleProducto>> FindDetailsByIdProduct(int idProduct);
        Task<string> EnableProduct(int idProduct);
        Task<string> UpdateStock(int idProduct, int stock);
        Task<Producto> FindProductById(int idProduct);
        Task<List<Producto>> GetSimpleProductsByIdSubCategories(int idSubCategory);
        Task<List<Producto>> GetAllProductsSubCategory();
    }
}

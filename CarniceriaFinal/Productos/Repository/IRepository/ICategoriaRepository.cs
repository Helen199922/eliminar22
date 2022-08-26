using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public interface ICategoriaRepository
    {
        Task<CategoriaProducto> GetCategoriaById(int id);
        Task<CategoriaProducto> CreateCategory(CategoriaProducto categoria);
        CategoriaProducto UpdateCategory(CategoriaProducto category);
        Task<Boolean> DelateCategoryById(int idCategory);
        Task<List<CategoriaProducto>> GetAllCategories();
        Task<List<CategoriaInProducto>> GetAllProductsInCategory(List<Producto> products, int idCategory);
        Task<List<CategoriaInProducto>> GetAllProductsExistInCategory(int idCategory);
        Task<CategoriaInProducto> CrearCategoriaInProduct(int idCategory, int idProduct);
        Task<List<CategoriaProducto>> GetAllCategoriesByProductId(int idProduct);

        Task<Boolean> ChangeStatusCategory(int idCategory, int idStatus);
        Task<Boolean> DeleteCategoriaInProduct(List<CategoriaInProducto> range);
    }
}

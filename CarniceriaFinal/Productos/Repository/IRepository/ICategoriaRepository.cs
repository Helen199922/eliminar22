using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
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
        Task<List<CategoriaProducto>> GetAllAdmCategories();
        Task<List<SimpleProductInSubCategory>> GetAllProductsByIdCategory(int idCategory);
        Task<List<SimpleProductInSubCategory>> GetAllProductsToCategory();
        
        Task<List<CategoriaInProducto>> GetAllProductsInCategory(List<Producto> products, int idCategory);
        Task<List<CategoriaInProducto>> GetAllProductsExistInCategory(int idCategory);
        Task<CategoriaInProducto> CrearCategoriaInProduct(int idCategory, int idProduct);
        Task<CategoriaProducto> GetCategoryByProductId(int idProduct);

        Task<Boolean> ChangeStatusCategory(int idCategory, int idStatus);
        Task<Boolean> DeleteCategoriaInProduct(List<CategoriaInProducto> range);
    }
}

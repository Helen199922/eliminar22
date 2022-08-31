using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.DTOs;
using CarniceriaFinal.Productos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.IServicios
{
    public interface ICategoriaService
    {
        Task<List<CategoriaProductoEntity>> CreateCategoryProcess(List<CategoriaProductoEntity> categories);
        Task<CategoriaProducto> CreateCategory(string titulo, string descripcion, string urlImage);
        Task<List<SimpleProductInSubCategory>> GetAllProductsByIdCategory(int idCategory);
        Task<List<SimpleProductInSubCategory>> GetAllProductsToCategory();


        Task<CategoriaProductoEntity> GetCategoryById(int idCategory);
        Task<List<CategoryEntity>> GetAllCategories();
        Task<List<CategoryEntity>> GetAllAdmCategories();
        Task<CategoriaProductoEntity> UpdateCategory(CategoriaProductoEntity category);
        Task<Boolean> ChangeStatusCategory(int idCategory, int idStatus);
        Task<List<CategoryEntity>> GetAllCategoriesByProductId(int idProduct);
    }
}

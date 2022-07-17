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
        Task<List<CategoriaProducto>> GetAllCategoriaAndSubCategory();
        Task<List<SubCategorium>> GetSubCategoryByCategoryId(int idCategory);
        CategoriaProducto UpdateCategory(CategoriaProducto category);
        Task<Boolean> UpdateCategoryInSubcategory(int idCategory, List<int> idSubCategories);
        Task<Boolean> DelateCategoryById(int idCategory);
        Task<SubCategorium> UpdateSubCategory(SubCategorium subCategory);
        Task<List<CategoriaProducto>> GetAllCategoriesByIdSubCategory(int idSubCategory);
        Task<List<SubInCategorium>> GetAllCategoriesAndSubCategoriesByProductId(int idProduct);
        Task<CategoriaProducto> GetCategoryById(int idCategory);
        Task<List<CategoriaProducto>> GetAllCategories();
    }
}

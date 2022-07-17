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
        Task<SubCategorium> CreateSubCategory(string titulo, string descripcion, string urlImage);
        Task<SubInCategorium> CreteSubInCategoria(int idCategoria, int idSubCategoria);
        Task<List<CategoriaProductoEntity>> GetAllCategoriaAndSubCategory();
        Task<CreateSubCategory> UpdateSubCategoryAndCategory(CreateSubCategory subCategory);

        Task<CategoriaProductoEntity> UpdateCategoryAndSubCategory(CategoriaProductoEntity category);
        Task<Boolean> DeleteCategoryAndSubCatAndRelationship(int idCategory);
        Task<List<CreateSubCategory>> CreateSubCategoriesProcess(List<CreateSubCategory> subCategories);
        Task<Boolean> DeleteSubCatAndCategoryRelationship(int idSubCategory);
        Task<List<CategoriaProductoEntity>> GetAllCategoriesAndSubCategoriesByProductId(int idProduct);
        string DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
            (int idProduct, int idSubCategory, int idCategory);
        Task<CategoriaProductoEntity> GetCategoryById(int idCategory);
        Task<CreateSubCategory> GetSubCategoryAndCategoryByIdSubCategory(int idSubCategory);
        Task<List<SubCategoriaAdminEntity>> GetAllSubCategories();
        Task<List<CategoryAdminEntity>> GetAllCategories();
    }
}

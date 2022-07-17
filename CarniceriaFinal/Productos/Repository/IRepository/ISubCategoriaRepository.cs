using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.IRepository
{
    public interface ISubCategoriaRepository
    {
        Task<SubCategorium> CreateSubCategory(SubCategorium subCat);
        SubCategorium UpdateSubCategory(SubCategorium subCategory);
        Task<Boolean> DeleteSubCategoryById(int idSubCategory);
        Task<SubCategorium> GetSubCategoriaById(int idSubCategory);
        Task<List<SubCategorium>> GetSubCategoriesByIdCategory(int idCategory);
        Task<Boolean> UpdateCategoryInSubcategory(int idSubCategory, List<int> idCategories);
        Task<List<SubCategorium>> GetAllSubCategories();
    }
}

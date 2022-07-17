using CarniceriaFinal.ModelsEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.IRepository
{
    public interface ISubInCategoriaRepository
    {
        Task<SubInCategorium> CrearSubInCategoria(SubInCategorium subCat);
        Task<List<SubInCategorium>> DeleteSubInCategoria(List<SubInCategorium> subCat);
        Task<List<SubInCategorium>> GetAllSubInCategoriesByCategoryId(int idCategory);
        Task<List<SubInCategorium>> GetAllSubInCategoriesBySubCategoryId(int idSubCategory);
        Task<List<SubInCategorium>> GetSubInCategoriesBySubCategoryIdAndCategoryId(int idSubCategory, int idCategory);
        Task<SubInCategorium> CrearSubInCategoria(int idCategory, int idSubCategory);
        string DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
            (int idProduct, int idSubCategory, int idCategory);
        Task<List<SubInCategorium>> GetAllProductsInSubCategory(List<Producto> products, int idSubCategory, int idCategory);
        Task<List<SubInCategorium>> GetAllProductsExistInSubCategory(int idSubCategory, int idCategory);
        Task<string> DeleteProductAndElementInSubCategory(int? idProduct, int idSubCategory, int idCategory);
    }
}

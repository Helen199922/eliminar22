using CarniceriaFinal.Core.CustomException;
using CarniceriaFinal.ModelsEF;
using CarniceriaFinal.Productos.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Productos.Repository
{
    public class SubInCategoriaRepository : ISubInCategoriaRepository
    {
        public readonly DBContext Context;
        public SubInCategoriaRepository(DBContext _Context)
        {
            Context = _Context;
        }
        public async Task<SubInCategorium> CrearSubInCategoria(int idCategory, int idSubCategory)
        {
            try
            {
                SubInCategorium subInCategory = new()
                {
                    IdCategoria = idCategory,
                    IdSubCategoria = idSubCategory
                };
                await Context.SubInCategoria.AddAsync(subInCategory);
                await Context.SaveChangesAsync();

                return subInCategory;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Crear Sub CategoriaInProduct");
            }

        }
        public async Task<SubInCategorium> CrearSubInCategoria(SubInCategorium subCat)
        {
            try
            {
                await Context.SubInCategoria.AddAsync(subCat);
                await Context.SaveChangesAsync();

                return subCat;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Crear Sub CategoriaInProduct");
            }

        }
        public async Task<List<SubInCategorium>> GetAllSubInCategoriesByCategoryId(int idCategory)
        {
            try
            {
                return await Context.SubInCategoria.Where(x => x.IdCategoria == idCategory)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener Sub Categorias");
            }

        }
        public async Task<List<SubInCategorium>> GetAllSubInCategoriesBySubCategoryId(int idSubCategory)
        {
            try
            {
                return await Context.SubInCategoria.Where(x => x.IdSubCategoria == idSubCategory)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener Categorias");
            }

        }
        public async Task<List<SubInCategorium>> GetSubInCategoriesBySubCategoryIdAndCategoryId(int idSubCategory, int idCategory)
        {
            try
            {
                return await Context.SubInCategoria.Where(x => x.IdSubCategoria == idSubCategory && x.IdCategoria == idCategory)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Obtener SubInCategoria y Categoria");
            }

        }
        public async Task<List<SubInCategorium>> DeleteSubInCategoria(List<SubInCategorium> subCat)
        {
            try
            {
                Context.SubInCategoria.RemoveRange(subCat);
                await Context.SaveChangesAsync();

                return subCat;
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Borrar Sub CategoriaInProduct");
            }

        }
        public string DeleteSubInCategorByProductIdAndSubCategoryIdAndCategoryId
            (int idProduct, int idSubCategory, int idCategory)
        {
            try
            {
                SubInCategorium subInCategory = new()
                {
                    IdProducto = idProduct,
                    IdSubCategoria = idSubCategory,
                    IdCategoria = idCategory
                };
                Context.SubInCategoria.Remove(subInCategory);
                Context.SaveChanges();

                return "Proceso realizado correctamente";
            }
            catch (Exception)
            {
                throw RSException.ErrorQueryDB("Eliminar Sub CategoriaInProduct");
            }

        }
        public async Task<List<SubInCategorium>> GetAllProductsInSubCategory(List<Producto> products, int idSubCategory, int idCategory)
        {
            try
            {
                List<int> productsId = new();
                foreach (var item in products)
                {
                    productsId.Add(item.IdProducto);
                }
                var productsInSubCategory = await Context.SubInCategoria
                    .Where(x => productsId.Contains(x.IdProducto.Value) && x.IdSubCategoria == idSubCategory && x.IdCategoria == idCategory)
                    .AsNoTracking()
                    .ToListAsync();

                return productsInSubCategory;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener los productos relacionados con subcategorias y categorias");
            }
        }
        public async Task<List<SubInCategorium>> GetAllProductsExistInSubCategory(int idSubCategory, int idCategory)
        {
            try
            {
                var productsInSubCategory = await Context.SubInCategoria
                    .Where(x => x.IdSubCategoria == idSubCategory && x.IdCategoria == idCategory)
                    .AsNoTracking()
                    .ToListAsync();

                return productsInSubCategory;
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("Obtener las relaciones con subcategorias y categorias");
            }
        }
        public async Task<string> DeleteProductAndElementInSubCategory(int? idProduct, int idSubCategory, int idCategory)
        {
            try
            {

                using (var _Context = new DBContext())
                {
                    var productInSubCategory = await _Context.SubInCategoria
                        .Where(x => x.IdProducto == idProduct && x.IdSubCategoria == idSubCategory && x.IdCategoria == idCategory)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();


                    _Context.SubInCategoria.Remove(new SubInCategorium(){
                        IdCategoria = idCategory,
                        IdProducto = idProduct,
                        IdSubCategoria = idSubCategory,
                        IdSubInCategory = productInSubCategory.IdSubInCategory
                    });
                    await _Context.SaveChangesAsync();
                }


                return "elemento eliminado correctamente";
            }
            catch (Exception err)
            {
                throw RSException.ErrorQueryDB("eliminar los productos relacionados con subcategorias y categorias");
            }
        }
    }
}
